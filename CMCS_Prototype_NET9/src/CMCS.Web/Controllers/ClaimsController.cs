using CMCS.Web.Models;
using CMCS.Web.Services;
using CMCS.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CMCS.Web.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly IClaimStore _store;
        private readonly IWebHostEnvironment _env;
        private readonly IHubContext<ClaimsHub> _hub;

        private static readonly string[] AllowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
        private const long MaxBytes = 10 * 1024 * 1024; // 10 MB

        public ClaimsController(IClaimStore store, IWebHostEnvironment env, IHubContext<ClaimsHub> hub)
        {
            _store = store;
            _env = env;
            _hub = hub;
        }

        public IActionResult Index()
        {
            return View(_store.All());
        }

        public IActionResult New()
        {
            return View(new ClaimCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(ClaimCreateViewModel vm)
        {
            try
            {
                if (vm.Document != null)
                {
                    if (vm.Document.Length > MaxBytes)
                        ModelState.AddModelError("Document", "File too large (max 10 MB).");

                    var ext = Path.GetExtension(vm.Document.FileName).ToLowerInvariant();
                    if (!AllowedExtensions.Contains(ext))
                        ModelState.AddModelError("Document", "Only .pdf, .docx, .xlsx files are allowed.");
                }

                if (!ModelState.IsValid)
                    return View(vm);

                string? storedName = null;
                if (vm.Document != null && vm.Document.Length > 0)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploads);
                    storedName = $"{Guid.NewGuid():N}{Path.GetExtension(vm.Document.FileName)}";
                    var dest = Path.Combine(uploads, storedName);
                    using var fs = System.IO.File.Create(dest);
                    await vm.Document.CopyToAsync(fs);
                }

                var claim = new Claim
                {
                    HoursWorked = vm.HoursWorked,
                    HourlyRate = vm.HourlyRate,
                    Notes = vm.Notes,
                    DocumentFileName = storedName,
                    Status = ClaimStatus.PendingVerification
                };
                _store.Add(claim);
                await _hub.Clients.All.SendAsync("statusChanged", claim.Id, claim.Status.ToString());

                TempData["Success"] = "Claim submitted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                return View(vm);
            }
        }

        public IActionResult Detail(string id)
        {
            var claim = _store.Get(id);
            if (claim == null) return NotFound();
            return View(claim);
        }
    }
}

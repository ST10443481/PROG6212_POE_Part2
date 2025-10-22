namespace CMCS.Web.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "Lecturer"; // Lecturer|Coordinator|Manager
        public bool IsActive { get; set; } = true;
    }
}

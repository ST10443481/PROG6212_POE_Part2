# PROG6212_POE_Part2
# 🧾 Monthly Claim System

The **Monthly Claim System** is a web-based application designed to help lecturers and administrators manage, submit, and review monthly claims efficiently. It provides an easy-to-use interface for creating, tracking, and approving claims, while ensuring transparency and accountability through digital record-keeping.

---

## 📋 Features

- 👩‍🏫 **Lecturer Claims Management**
  - Create, edit, and submit monthly claims.
  - Attach supporting documents (e.g., PDFs, timesheets).
- 🧑‍💼 **Admin Review and Approval**
  - Approve or reject claims with reasons and history tracking.
- 🧾 **In-Memory Data Storage**
  - The current implementation uses an `InMemoryClaimRepository` for demonstration and testing purposes.
- 📄 **Role-Based Access**
  - Supports different user roles (Lecturer, Admin, etc.).
- 📂 **Audit Trail**
  - Records claim status changes and approval history.

---

🧱 Project Structure
MonthlyClaimSystem/
├── Controllers/
│ ├── ClaimController.cs
│ ├── AccountController.cs
│ └── HomeController.cs
├── Data/
│ ├── IClaimRepository.cs
│ └── InMemoryClaimRepository.cs
├── Models/
│ ├── Claim.cs
│ ├── SupportingDocument.cs
│ ├── User.cs
│ └── ApprovalHistory.cs
├── Views/
├── wwwroot/
└── MonthlyClaimSystem.csproj



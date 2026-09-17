# Second-Year--Final-project-

# Contract Monthly Claim System (CMCS)

An ASP.NET Core MVC web application that lets independent contractor lecturers submit monthly claims for hours worked, and routes those claims through a coordinator/academic manager approval workflow before HR processes payment.

**Author:** Buchule Mqingwana (ST10367481)
**Stack:** ASP.NET Core MVC (.NET), Bootstrap 5, jQuery, jQuery Validation

---

## Overview

CMCS supports four user roles, each with a dedicated dashboard and set of actions:

| Role | Can do |
|---|---|
| **Lecturer** | Submit a new claim, view their own claim history and status |
| **Coordinator** | Review pending (submitted) claims, approve or reject with comments |
| **Academic Manager** | View all claims across the system, view summary reports |
| **HR** | Manage lecturer records, view/process payments for approved claims |

Authentication and role checks are handled via session state, with each controller guarding its actions behind a role check before allowing access.

---

## Features

- **Login / Logout** - session-based authentication (`AccountController`)
- **Role-aware dashboard** - shows live totals for claims, approvals, pending items and payments (`HomeController`)
- **Claim submission** - lecturers submit module name, hours worked, and hourly rate; the total amount is calculated automatically (`LecturerController`)
- **Claim history** - lecturers can view all claims they've submitted and their current status
- **Approval workflow** - coordinators approve or reject submitted claims, with rejection comments captured and an approver/timestamp recorded (`CoordinatorController`)
- **Reporting** - academic managers get an all-claims view plus aggregate stats (total claims, approved, pending, total payments) (`AcademicManagerController`)
- **Lecturer management** - HR can add new lecturers, which automatically provisions a linked user account (`HRController`)
- **Payment processing** - HR can view approved claims awaiting payment and mark them as paid in bulk (`HRController`)
- **Server-side validation** - model validation via Data Annotations (required fields, string length, numeric ranges, email format)

---

## Claim Lifecycle

```
Submitted - Approved - Paid
         ↘ Rejected
```

- `Submitted` - created by a lecturer, awaiting coordinator review
- `Approved` - accepted by a coordinator, awaiting HR payment
- `Rejected` - declined by a coordinator, with comments explaining why
- `Paid` - processed by HR in the Payment Report

---

## Project Structure

```
PART 3/
├── Controllers/
│   ├── AccountController.cs           # Login / logout
│   ├── HomeController.cs              # Dashboard (role-aware landing page)
│   ├── LecturerController.cs          # Submit claim, view own claims
│   ├── CoordinatorController.cs       # Review, approve/reject claims
│   ├── AcademicManagerController.cs   # All claims view + reports
│   └── HRController.cs                # Lecturer management + payments
├── Models/
│   ├── Claim.cs                       # Claim entity + ClaimStatus enum
│   ├── Lecturer.cs                    # Lecturer entity
│   ├── User.cs                        # User entity + UserRole enum
│   └── ErrorViewModel.cs
├── appsettings.json
└── appsettings.Development.json
```

> Data access is abstracted behind an `IDataService` interface injected into every controller, keeping the controllers independent of how claims, lecturers and users are actually stored.

---

## Data Models

**Claim**
- `ModuleName` (required, max 100 chars)
- `HoursWorked` (required, 1–200)
- `HourlyRate` (required, 0–1000)
- `TotalAmount` - computed (`HoursWorked × HourlyRate`)
- `ClaimDate`, `SubmittedDate`, `ApprovedDate`, `ApprovedBy`, `Comments`
- `Status` - `Submitted` / `Approved` / `Rejected` / `Paid`

**Lecturer**
- `FirstName`, `LastName`, `Email` (validated), `Department`, `PhoneNumber`, `DateCreated`

**User**
- `Username`, `Password`, `Email`, `Role` (`Lecturer` / `Coordinator` / `AcademicManager` / `HR`), `LecturerId`

---

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (6.0 or later)
- Visual Studio 2022 (or VS Code with the C# extension)

### Run locally

```bash
# from the project directory (where the .csproj file lives)
dotnet restore
dotnet run
```

Then open the URL shown in the console (typically `https://localhost:xxxx`) and log in with a seeded user account.

---

## Notes & Known Limitations

- Passwords are currently stored in plain text and lecturer accounts are created with a placeholder password - this is fine for a class demo but **not** suitable for production use.
- Data is served through `IDataService`; if this is backed by an in-memory store rather than a database, claims and lecturers will reset when the app restarts.
- Role checks are implemented manually per action rather than via ASP.NET Core's built-in `[Authorize]`/policy system.


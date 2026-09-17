using Microsoft.AspNetCore.Mvc;
using PART_3.Models;
using PART_3.Services;

namespace PART_3.Controllers
{
    public class HRController : Controller
    {
        private readonly IDataService _dataService;

        public HRController(IDataService dataService)
        {
            _dataService = dataService;
        }

        public IActionResult Lecturers()
        {
            if (!IsAuthorized(UserRole.HR))
                return RedirectToAction("Login", "Account");

            var lecturers = _dataService.GetLecturers();
            return View(lecturers);
        }

        public IActionResult AddLecturer()
        {
            if (!IsAuthorized(UserRole.HR))
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddLecturer(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                _dataService.AddLecturer(lecturer);

                // Also create a user account for the lecturer
                var user = new User
                {
                    Username = lecturer.Email.Split('@')[0],
                    Password = "temp123", // In real app, generate random password
                    Email = lecturer.Email,
                    Role = UserRole.Lecturer,
                    LecturerId = lecturer.Id
                };
                _dataService.AddUser(user);

                TempData["Success"] = "Lecturer added successfully!";
                return RedirectToAction("Lecturers");
            }

            return View(lecturer);
        }

        public IActionResult PaymentReport()
        {
            if (!IsAuthorized(UserRole.HR))
                return RedirectToAction("Login", "Account");

            var approvedClaims = _dataService.GetApprovedClaimsForPayment();
            ViewBag.TotalAmount = approvedClaims.Sum(c => c.TotalAmount);
            return View(approvedClaims);
        }

        [HttpPost]
        public IActionResult ProcessPayments()
        {
            if (!IsAuthorized(UserRole.HR))
                return RedirectToAction("Login", "Account");

            var approvedClaims = _dataService.GetApprovedClaimsForPayment();
            foreach (var claim in approvedClaims)
            {
                claim.Status = ClaimStatus.Paid;
                _dataService.UpdateClaim(claim);
            }

            TempData["Success"] = $"Payments processed for {approvedClaims.Count} claims!";
            return RedirectToAction("PaymentReport");
        }

        private bool IsAuthorized(UserRole requiredRole)
        {
            var userRole = HttpContext.Session.GetString("Role");
            return !string.IsNullOrEmpty(userRole) &&
                   Enum.Parse<UserRole>(userRole) == requiredRole;
        }
    }
}
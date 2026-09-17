using Microsoft.AspNetCore.Mvc;
using PART_3.Models;
using PART_3.Services;

namespace PART_3.Controllers
{
    public class AcademicManagerController : Controller
    {
        private readonly IDataService _dataService;

        public AcademicManagerController(IDataService dataService)
        {
            _dataService = dataService;
        }

        public IActionResult AllClaims()
        {
            if (!IsAuthorized(UserRole.AcademicManager))
                return RedirectToAction("Login", "Account");

            var claims = _dataService.GetClaims();
            return View(claims);
        }

        public IActionResult Reports()
        {
            if (!IsAuthorized(UserRole.AcademicManager))
                return RedirectToAction("Login", "Account");

            var claims = _dataService.GetClaims();
            var totalClaims = claims.Count;
            var approvedClaims = claims.Count(c => c.Status == ClaimStatus.Approved);
            var pendingClaims = claims.Count(c => c.Status == ClaimStatus.Submitted);
            var totalPayments = claims.Where(c => c.Status == ClaimStatus.Approved).Sum(c => c.TotalAmount);

            ViewBag.TotalClaims = totalClaims;
            ViewBag.ApprovedClaims = approvedClaims;
            ViewBag.PendingClaims = pendingClaims;
            ViewBag.TotalPayments = totalPayments;

            return View(claims);
        }

        private bool IsAuthorized(UserRole requiredRole)
        {
            var userRole = HttpContext.Session.GetString("Role");
            return !string.IsNullOrEmpty(userRole) &&
                   Enum.Parse<UserRole>(userRole) == requiredRole;
        }
    }
}
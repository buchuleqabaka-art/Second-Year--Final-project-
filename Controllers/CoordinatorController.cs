using Microsoft.AspNetCore.Mvc;
using PART_3.Models;
using PART_3.Services;

namespace PART_3.Controllers
{
    public class CoordinatorController : Controller
    {
        private readonly IDataService _dataService;

        public CoordinatorController(IDataService dataService)
        {
            _dataService = dataService;
        }

        public IActionResult PendingClaims()
        {
            if (!IsAuthorized(UserRole.Coordinator))
                return RedirectToAction("Login", "Account");

            var claims = _dataService.GetClaimsByStatus(ClaimStatus.Submitted);
            return View(claims);
        }

        [HttpPost]
        public IActionResult ApproveClaim(int id)
        {
            if (!IsAuthorized(UserRole.Coordinator))
                return RedirectToAction("Login", "Account");

            var claim = _dataService.GetClaim(id);
            if (claim != null)
            {
                claim.Status = ClaimStatus.Approved;
                claim.ApprovedDate = DateTime.Now;
                claim.ApprovedBy = HttpContext.Session.GetString("Username");
                _dataService.UpdateClaim(claim);

                TempData["Success"] = "Claim approved successfully!";
            }

            return RedirectToAction("PendingClaims");
        }

        [HttpPost]
        public IActionResult RejectClaim(int id, string comments)
        {
            if (!IsAuthorized(UserRole.Coordinator))
                return RedirectToAction("Login", "Account");

            var claim = _dataService.GetClaim(id);
            if (claim != null)
            {
                claim.Status = ClaimStatus.Rejected;
                claim.Comments = comments;
                claim.ApprovedDate = DateTime.Now;
                claim.ApprovedBy = HttpContext.Session.GetString("Username");
                _dataService.UpdateClaim(claim);

                TempData["Success"] = "Claim rejected successfully!";
            }

            return RedirectToAction("PendingClaims");
        }

        private bool IsAuthorized(UserRole requiredRole)
        {
            var userRole = HttpContext.Session.GetString("Role");
            return !string.IsNullOrEmpty(userRole) &&
                   Enum.Parse<UserRole>(userRole) == requiredRole;
        }
    }
}
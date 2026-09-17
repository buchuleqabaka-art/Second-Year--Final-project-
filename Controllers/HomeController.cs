using Microsoft.AspNetCore.Mvc;
using PART_3.Services;
using System.Diagnostics;

namespace PART_3.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataService _dataService;

        public HomeController(IDataService dataService)
        {
            _dataService = dataService;
        }

        public IActionResult Dashboard()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
                return RedirectToAction("Login", "Account");

            var role = HttpContext.Session.GetString("Role");
            ViewBag.Role = role;
            ViewBag.Username = HttpContext.Session.GetString("Username");

            // Dashboard statistics
            var claims = _dataService.GetClaims();
            var lecturers = _dataService.GetLecturers();

            ViewBag.TotalClaims = claims.Count;
            ViewBag.PendingClaims = claims.Count(c => c.Status == Models.ClaimStatus.Submitted);
            ViewBag.ApprovedClaims = claims.Count(c => c.Status == Models.ClaimStatus.Approved);
            ViewBag.TotalPayments = _dataService.GetTotalPaymentsPending();
            ViewBag.TotalLecturers = lecturers.Count;

            return View();
        }

        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }
    }
}
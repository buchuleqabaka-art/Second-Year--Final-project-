using Microsoft.AspNetCore.Mvc;
using PART_3.Models;
using PART_3.Services;
using System.Diagnostics;

namespace PART_3.Controllers
{
    public class LecturerController : Controller
    {
        private readonly IDataService _dataService;

        public LecturerController(IDataService dataService)
        {
            _dataService = dataService;
        }

        // GET: /Lecturer/SubmitClaim
        [HttpGet]
        public IActionResult SubmitClaim()
        {
            Console.WriteLine("🔵 GET SubmitClaim called");

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
                return RedirectToAction("Login", "Account");

            var lecturerId = HttpContext.Session.GetInt32("LecturerId");
            Console.WriteLine($"🔵 Session - LecturerId: {lecturerId}");

            if (lecturerId == null || lecturerId == 0)
                return RedirectToAction("Login", "Account");

            // Return form with today's date
            var model = new Claim { ClaimDate = DateTime.Today };
            return View(model);
        }

        // POST: /Lecturer/SubmitClaim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitClaim(Claim claim)
        {
            Console.WriteLine("🔴 POST SubmitClaim called - START");

            if (ModelState.IsValid)
            {
                var lecturerId = HttpContext.Session.GetInt32("LecturerId");
                Console.WriteLine($"🔴 Session LecturerId: {lecturerId}");

                if (lecturerId != null && lecturerId > 0)
                {
                    claim.LecturerId = lecturerId.Value;
                    claim.Status = ClaimStatus.Submitted;
                    claim.SubmittedDate = DateTime.Now;

                    Console.WriteLine($"🔴 Adding claim: {claim.ModuleName}");

                    _dataService.AddClaim(claim);

                    TempData["SuccessMessage"] = $"Claim submitted successfully!";
                    return RedirectToAction("MyClaims");
                }
            }

            Console.WriteLine("🔴 POST SubmitClaim - END (with errors)");
            return View(claim);
        }

        // GET: /Lecturer/MyClaims
        public IActionResult MyClaims()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
                return RedirectToAction("Login", "Account");

            var lecturerId = HttpContext.Session.GetInt32("LecturerId");
            if (lecturerId == null || lecturerId == 0)
                return RedirectToAction("Login", "Account");

            var claims = _dataService.GetClaimsByLecturer(lecturerId.Value);
            return View(claims);
        }
    }
}
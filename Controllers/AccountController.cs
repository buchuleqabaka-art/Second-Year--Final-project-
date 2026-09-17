using Microsoft.AspNetCore.Mvc;
using PART_3.Models;
using PART_3.Services;
using System.Diagnostics;

namespace PART_3.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDataService _dataService;

        public AccountController(IDataService dataService)
        {
            _dataService = dataService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _dataService.Authenticate(username, password);
            if (user != null)
            {
                // Store user info in session
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role.ToString());
                HttpContext.Session.SetInt32("LecturerId", user.LecturerId ?? 0);

                return RedirectToAction("Dashboard", "Home");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
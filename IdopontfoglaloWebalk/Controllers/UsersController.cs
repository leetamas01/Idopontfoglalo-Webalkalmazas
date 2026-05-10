using IdopontfoglaloWebalk.Context;
using IdopontfoglaloWebalk.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdopontfoglaloWebalk.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly EfContext _context;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, EfContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(
                        user.UserName,
                        password,
                        rememberMe,
                        lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ViewBag.ErrorMessage = "Helytelen email cím vagy jelszó!";
            }
            return View("~/Views/Users/Login.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> Registration(User user, string password)
        {
            user.rating = 0;
            user.service = "Customer";

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Users");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View("Registration", user);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Registration()
        {
            return View();
        }
    }
}

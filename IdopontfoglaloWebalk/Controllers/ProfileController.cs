using IdopontfoglaloWebalk.Context;
using IdopontfoglaloWebalk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    private readonly EfContext _context;

    public ProfileController(UserManager<User> userManager, SignInManager<User> signInManager, EfContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return RedirectToAction("Login", "Home");
        }

        return View(user);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(string email, string phoneNumber)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        user.Email = email;
        user.NormalizedEmail = email.ToUpper();
        user.PhoneNumber = phoneNumber;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            await _signInManager.RefreshSignInAsync(user);

            TempData["StatusMessage"] = "A profilod sikeresen frissült!";
            return RedirectToAction(nameof(Profile));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View("Profile", user);
    }
}

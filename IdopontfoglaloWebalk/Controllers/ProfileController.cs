using IdopontfoglaloWebalk.Context;
using IdopontfoglaloWebalk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<Users> _userManager;
    private readonly SignInManager<Users> _signInManager;
    private readonly EfContext _context;

    public ProfileController(UserManager<Users> userManager, SignInManager<Users> signInManager, EfContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Home");
        }

        var reservations = await _context.Occasions
            .Include(o => o.Service)
            .Include(o => o.ServiceCategory)
            .Where(o => o.user_id == user.Id)
            .OrderBy(o => o.date)
            .ToListAsync();

        var viewModel = new ProfileViewModel
        {
            User = user,
            Reservations = reservations
        };

        return View(viewModel);
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
            TempData["StatusMessage"] = "A profilod adatai sikeresen frissültek!";
            return RedirectToAction(nameof(Profile));
        }

        TempData["ErrorMessage"] = string.Join(" ", result.Errors.Select(e => e.Description));
        return RedirectToAction(nameof(Profile));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelReservation(int reservationId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var reservation = await _context.Occasions
            .FirstOrDefaultAsync(o => o.reservation_id == reservationId && o.user_id == user.Id);

        if (reservation == null)
        {
            TempData["ErrorMessage"] = "A lemondani kívánt foglalás nem található!";
            return RedirectToAction(nameof(Profile));
        }

        reservation.user_id = null;
        reservation.reservation_date = null;
        reservation.status = "Szabad";

        await _context.SaveChangesAsync();
        TempData["StatusMessage"] = "A foglalás sikeresen lemondva!";
        return RedirectToAction(nameof(Profile));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
    {
        if (newPassword != confirmPassword)
        {
            TempData["ErrorMessage"] = "Az új jelszó és a megerősítés nem egyezik meg!";
            return RedirectToAction(nameof(Profile));
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (result.Succeeded)
        {
            await _signInManager.RefreshSignInAsync(user);
            TempData["StatusMessage"] = "A jelszavad sikeresen megváltozott!";
            return RedirectToAction(nameof(Profile));
        }

        TempData["ErrorMessage"] = string.Join(" ", result.Errors.Select(e => e.Description));
        return RedirectToAction(nameof(Profile));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProfile(string password)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        //Csak a sima felhasználók törölhetik magukat
        if (await _userManager.IsInRoleAsync(user, "Admin"))
        {
            TempData["ErrorMessage"] = "A rendszergazdai (Admin) fiók biztonsági okokból nem törölhető!";
            return RedirectToAction(nameof(Profile));
        }

        //Jelszó megerősítés
        if (string.IsNullOrWhiteSpace(password) || !await _userManager.CheckPasswordAsync(user, password))
        {
            TempData["ErrorMessage"] = "A törléshez megadott jelszó helytelen!";
            return RedirectToAction(nameof(Profile));
        }

        //Ellenőrzés: Van-e még aktív szolgáltatása?
        bool hasServices = await _context.Services.AnyAsync(s => s.owner_id == user.Id);
        if (hasServices)
        {
            TempData["ErrorMessage"] = "Nem törölheted a fiókodat, amíg van aktív szolgáltatásod!";
            return RedirectToAction(nameof(Profile));
        }

        //Foglalások felszabadítása
        var reservations = await _context.Occasions.Where(o => o.user_id == user.Id).ToListAsync();
        foreach (var r in reservations)
        {
            r.user_id = null;
            r.reservation_date = null;
            r.status = "Szabad";
        }
        await _context.SaveChangesAsync();

        //Kijelentkeztetés és törlés
        await _signInManager.SignOutAsync();
        await _userManager.DeleteAsync(user);

        return RedirectToAction("Index", "Home");
    }
}
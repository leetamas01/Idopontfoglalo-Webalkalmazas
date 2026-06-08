using IdopontfoglaloWebalk.Context;
using IdopontfoglaloWebalk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdopontfoglaloWebalk.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {
        private readonly EfContext _context;
        private readonly UserManager<Users> _userManager;

        public ServicesController(EfContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Services/MyServices
        public async Task<IActionResult> MyServices()
        {
            var currentUserId = _userManager.GetUserId(User);

            var myServices = await _context.Services
                .Where(s => s.owner_id == currentUserId)
                .ToListAsync();

            return View(myServices);
        }

        // GET: Services/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();

            ViewBag.CategoryList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories, "category_id", "category_name");

            return View();
        }

        // POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Services service)
        {
            ModelState.Remove("owner_id");
            ModelState.Remove("Owner");
            ModelState.Remove("Category");
            ModelState.Remove("ServiceCategories");

            if (ModelState.IsValid)
            {
                service.owner_id = _userManager.GetUserId(User);
                service.rating = 0.0;

                _context.Services.Add(service);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyServices));
            }

            var allCategories = await _context.Categories.ToListAsync();
            ViewBag.CategoryList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(allCategories, "category_id", "category_name");
            return View(service);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            if (service.owner_id != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            var categories = await _context.Categories.ToListAsync();
            ViewBag.CategoryList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories, "category_id", "category_name", service.category_id);

            return View(service);
        }

        // POST: Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Services service)
        {
            if (id != service.service_id) return NotFound();

            ModelState.Remove("owner_id");
            ModelState.Remove("Owner");
            ModelState.Remove("Category");
            ModelState.Remove("ServiceCategories");

            if (ModelState.IsValid)
            {
                try
                {
                    var originalService = await _context.Services.AsNoTracking().FirstOrDefaultAsync(s => s.service_id == id);
                    if (originalService == null || originalService.owner_id != _userManager.GetUserId(User))
                    {
                        return Forbid();
                    }

                    service.owner_id = originalService.owner_id;
                    service.rating = originalService.rating;

                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Services.Any(e => e.service_id == service.service_id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(MyServices));
            }

            var categories = await _context.Categories.ToListAsync();
            ViewBag.CategoryList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories, "category_id", "category_name", service.category_id);
            return View(service);
        }

        // GET: Services/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _context.Services
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.service_id == id);

            if (service == null) return NotFound();

            if (service.owner_id != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            if (service.owner_id != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyServices));
        }
        // GET: Services/Calendar/5
        [HttpGet]
        public async Task<IActionResult> Calendar(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            if (service.owner_id != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            return View(service);
        }

        [HttpGet]
        public async Task<JsonResult> GetCalendarEvents(int service_id, DateTime start, DateTime end)
        {
            var occasions = await _context.Occasions
                .Include(o => o.ServiceCategory)
                .Where(o => o.service_id == service_id && o.date >= start && o.date <= end)
                .ToListAsync();

            var eventList = occasions.Select(o => new
            {
                id = o.reservation_id,

                title = o.status == "reserved"
                    ? "FOGLALT: " + (o.ServiceCategory != null ? o.ServiceCategory.Name : "Szolgáltatás")
                    : (o.ServiceCategory != null ? o.ServiceCategory.Name : "Szabad időpont"),

                start = o.date.ToString("yyyy-MM-ddTHH:mm:ss"),

                end = o.date.AddMinutes(30).ToString("yyyy-MM-ddTHH:mm:ss"),

                backgroundColor = o.status == "reserved" ? "#dc3545" : "#198754",
                borderColor = o.status == "reserved" ? "#dc3545" : "#198754",

                extendedProps = new
                {
                    status = o.status,
                    userId = o.user_id
                }
            });

            return Json(eventList);
        }
    }
}
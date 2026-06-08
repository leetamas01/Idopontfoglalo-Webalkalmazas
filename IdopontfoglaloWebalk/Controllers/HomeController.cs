using IdopontfoglaloWebalk.Context;
using IdopontfoglaloWebalk.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace IdopontfoglaloWebalk.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EfContext _context;
        public HomeController(ILogger<HomeController> logger, EfContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Search(string searchQuery)
        {
            ViewBag.Query = searchQuery;

            if (string.IsNullOrEmpty(searchQuery))
            {
                return View(new List<Services>());
            }

            var results = await _context.Services
                .Include(s => s.Category)
                .Where(s => s.name.Contains(searchQuery) ||
                            s.description.Contains(searchQuery) ||
                            s.Category.category_name.Contains(searchQuery))
                .ToListAsync();

            return View(results);
        }

        // GET: Home/BookAppointment/5
        [HttpGet]
        public async Task<IActionResult> BookAppointment(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

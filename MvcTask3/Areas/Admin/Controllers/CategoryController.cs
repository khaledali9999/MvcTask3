using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcTask3.DataAccses;
using MvcTask3.Models;

namespace MvcTask3.Areas.Admin.Controllers
{
    [Area("Admin")]


    public class CategoryController : Controller
    {
        ApplicationDbContext _context = new();
        public IActionResult Index()
        {
            var categories = _context.Categories.AsNoTracking().AsQueryable();
            return View(categories.AsQueryable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();

           
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(e => e.id == id);

            if (category is null)
                return RedirectToAction("NotFoundPage", "Home");

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(e => e.id == id);

            if (category is null)
                return RedirectToAction("NotFoundPage", "Home");

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
   

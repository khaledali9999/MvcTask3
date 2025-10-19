using Microsoft.AspNetCore.Mvc;
using MvcTask3.DataAccses;
using System.Linq;

namespace MvcTask3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AnalysisController : Controller
    {
        private readonly ApplicationDbContext _context = new();

        public IActionResult Index()
        {
            
            var totalMovies = _context.Movies.Count();
            var totalCinemas = _context.Cinemas.Count();
            var totalActors = _context.Actors.Count();
            var totalCategories = _context.Categories.Count();

            
            var activeMovies = _context.Movies.Count(m => m.Status == true);

            
            var inactiveMovies = _context.Movies.Count(m => m.Status == false);

           
            var avgPrice = _context.Movies.Any() ? _context.Movies.Average(m => m.Price) : 0;

          
            var latestMovies = _context.Movies
                .OrderByDescending(m => m.dateTime)
                .Take(5)
                .ToList();

          
            ViewBag.TotalMovies = totalMovies;
            ViewBag.TotalCinemas = totalCinemas;
            ViewBag.TotalActors = totalActors;
            ViewBag.TotalCategories = totalCategories;
            ViewBag.ActiveMovies = activeMovies;
            ViewBag.InactiveMovies = inactiveMovies;
            ViewBag.AvgPrice = avgPrice;
            ViewBag.LatestMovies = latestMovies;

            return View();
        }
    }
}

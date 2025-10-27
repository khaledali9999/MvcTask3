using Microsoft.AspNetCore.Mvc;
using MvcTask3.Models;
using MvcTask3.Repos;
using System.Linq.Expressions;

namespace MvcTask3.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IRepository<Movie> _movieRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Cinema> _cinemaRepository;

        public HomeController(
            IRepository<Movie> movieRepository,
            IRepository<Category> categoryRepository,
            IRepository<Cinema> cinemaRepository)
        {
            _movieRepository = movieRepository;
            _categoryRepository = categoryRepository;
            _cinemaRepository = cinemaRepository;
        }

    
        public async Task<IActionResult> Index(
            int? cinemaId, int? categoryId, int page = 1, int pageSize = 3, CancellationToken cancellationToken = default)
        {
           
            var allMovies = await _movieRepository.GetAsync(
                includes: new Expression<Func<Movie, object>>[]
                {
                    m => m.Category,
                    m => m.Cinema
                },
                tracked: false,
                cancellationToken: cancellationToken);

           
            if (cinemaId.HasValue)
                allMovies = allMovies.Where(m => m.CinemaId == cinemaId.Value);

       
            if (categoryId.HasValue)
                allMovies = allMovies.Where(m => m.CategoryId == categoryId.Value);

            var totalMovies = allMovies.Count();
            var movies = allMovies
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

           
            var cinemas = await _cinemaRepository.GetAsync(tracked: false, cancellationToken: cancellationToken);
            var categories = await _categoryRepository.GetAsync(tracked: false, cancellationToken: cancellationToken);

            ViewBag.Cinemas = cinemas;
            ViewBag.Categories = categories;
            ViewBag.SelectedCinema = cinemaId;
            ViewBag.SelectedCategory = categoryId;

           
            ViewBag.TotalPages = (int)Math.Ceiling(totalMovies / (double)pageSize);
            ViewBag.CurrentPage = page;

            return View(movies);
        }

        
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetOneAsync(
                m => m.Id == id,
                includes: new Expression<Func<Movie, object>>[]
                {
                    m => m.Category,
                    m => m.Cinema
                },
                tracked: false,
                cancellationToken: cancellationToken);

            if (movie == null)
                return NotFound();

            return View(movie);
        }
    }
}

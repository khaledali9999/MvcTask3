using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcTask3.DataAccses;
using MvcTask3.Models;

namespace MvcTask3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context = new();

       
        public IActionResult Index(string? name, int? categoryId, int? cinemaId, int page = 1)
        {
            var movies = _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .Include(m => m.ActorMovies)
                .AsQueryable();

            
            if (!string.IsNullOrWhiteSpace(name))
            {
                movies = movies.Where(m => m.Name.Contains(name.Trim()));
                ViewBag.name = name;
            }

            if (categoryId is not null)
            {
                movies = movies.Where(m => m.CategoryId == categoryId);
                ViewBag.categoryId = categoryId;
            }

            if (cinemaId is not null)
            {
                movies = movies.Where(m => m.CinemaId == cinemaId);
                ViewBag.cinemaId = cinemaId;
            }

            
            ViewBag.TotalPages = Math.Ceiling(movies.Count() / 8.0);
            ViewBag.CurrentPage = page;
            movies = movies.Skip((page - 1) * 8).Take(8);

            
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Cinemas = _context.Cinemas.ToList();

            return View(movies.AsEnumerable());
        }

        
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Cinemas = _context.Cinemas.ToList();
            ViewBag.Actors = _context.Actors.ToList();
            return View();
        }

        
        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile img, List<IFormFile>? subImgs)
        {
            
            if (img is not null && img.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = System.IO.File.Create(filePath))
                    img.CopyTo(stream);

                movie.MainImg = fileName;
            }

            _context.Movies.Add(movie);
            _context.SaveChanges();

            
            if (subImgs is not null && subImgs.Count > 0)
            {
                foreach (var item in subImgs)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/movies");

                   
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = System.IO.File.Create(filePath))
                        item.CopyTo(stream);


                    _context.MovieSubImages.Add(new MovieSubImage
                    {
                        Img = fileName,
                        MovieId = movie.Id
                    });
                }

                _context.SaveChanges();
            }

            TempData["Notification"] = "Movie added successfully!";
            return RedirectToAction(nameof(Index));
        }

        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _context.Movies.FirstOrDefault(e => e.Id == id);
            if (movie == null)
                return RedirectToAction("NotFoundPage", "Home");

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Cinemas = _context.Cinemas.ToList();
            ViewBag.Actors = _context.Actors.ToList();

            return View(movie);
        }

        
        [HttpPost]
        public IActionResult Edit(Movie movie, IFormFile? img)
        {
            var movieInDb = _context.Movies.AsNoTracking().FirstOrDefault(e => e.Id == movie.Id);
            if (movieInDb == null)
                return RedirectToAction("NotFoundPage", "Home");

            if (img is not null && img.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = System.IO.File.Create(filePath))
                    img.CopyTo(stream);

                
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", movieInDb.MainImg);
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);

                movie.MainImg = fileName;
            }
            else
            {
                movie.MainImg = movieInDb.MainImg;
            }

            _context.Movies.Update(movie);
            _context.SaveChanges();

            TempData["Notification"] = "Movie updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.FirstOrDefault(e => e.Id == id);
            if (movie == null)
                return RedirectToAction("NotFoundPage", "Home");

            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", movie.MainImg);
            if (System.IO.File.Exists(oldPath))
                System.IO.File.Delete(oldPath);

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            TempData["Notification"] = "Movie deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}

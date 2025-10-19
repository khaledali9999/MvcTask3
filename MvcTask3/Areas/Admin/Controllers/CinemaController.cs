using Microsoft.AspNetCore.Mvc;
using MvcTask3.DataAccses;
using MvcTask3.Models;

namespace MvcTask3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        ApplicationDbContext _context = new();

        public IActionResult Index()
        {
            var Cinemas = _context.Cinemas.AsQueryable();

            

            return View(Cinemas.AsEnumerable());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Cinema cinema, IFormFile img)
        {
            if (img != null && img.Length > 0)
            {
                 
                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var filePath = Path.Combine(imagesFolder, fileName);

                
                using (var stream = System.IO.File.Create(filePath))
                {
                    img.CopyTo(stream);
                }

                 
                cinema.Img = fileName;
            }

            _context.Cinemas.Add(cinema);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Cinema = _context.Cinemas.FirstOrDefault(e => e.Id == id);

            if (Cinema is null)
                return RedirectToAction("NotFoundPage", "Home");

            return View(Cinema);
        }

        [HttpPost]
        public IActionResult Edit(Cinema Cinema, IFormFile? img)
        {
            var CinemaInDb = _context.Cinemas.FirstOrDefault(e => e.Id == Cinema.Id);
            if (CinemaInDb is null)
                return RedirectToAction("NotFoundPage", "Home");

             
            if (img is not null && img.Length > 0)
            {
                
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    img.CopyTo(stream);
                }

               
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", CinemaInDb.Img ?? "");
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                Cinema.Img = fileName;
            }
            else
            {
                Cinema.Img = CinemaInDb.Img; 
            }

             
            _context.Cinemas.Update(Cinema);
            _context.SaveChanges();

             
            return RedirectToAction("Index");  
        }

        public IActionResult Delete(int id)
        {
            var cinema = _context.Cinemas.Find(id);
            if (cinema == null)
            {
                return NotFound(); 
            }

            _context.Cinemas.Remove(cinema);
            _context.SaveChanges();

            return RedirectToAction("Index");  
        }


    }
}
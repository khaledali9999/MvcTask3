using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcTask3.DataAccses;
using MvcTask3.Models;

namespace MvcTask3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        ApplicationDbContext _context = new();
        public IActionResult Index()
        {
            var Actor = _context.Actors.AsNoTracking().AsQueryable();
            return View(Actor.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Actor actor, IFormFile img) {
            if (img != null && img.Length > 0)
            {
                // تأكد إن فولدر images موجود
                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                // اسم الملف الجديد
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var filePath = Path.Combine(imagesFolder, fileName);

                // حفظ الصورة
                using (var stream = System.IO.File.Create(filePath))
                {
                    img.CopyTo(stream);
                }

                // حفظ اسم الملف في قاعدة البيانات
                actor.Img = fileName;
            }

            _context.Actors.Add(actor);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]

        public IActionResult Edit(int id) {
            var actor = _context.Actors.FirstOrDefault(e => e.Id == id);

            if (actor is null)
                return RedirectToAction("NotFoundPage", "Home");

            return View(actor);
        }
        [HttpPost]
        public IActionResult Edit(Actor actor, IFormFile? img)
        {
            var actorInDb = _context.Actors.FirstOrDefault(e => e.Id == actor.Id);
            if (actorInDb is null)
                return RedirectToAction("NotFoundPage", "Home");

           
            if (img is not null && img.Length > 0)
            {
               
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    img.CopyTo(stream);
                }

               
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", actorInDb.Img ?? "");
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                actor.Img = fileName;
            }
            else
            {
                actor.Img = actorInDb.Img; 
            }

           
            _context.Actors.Update(actor);
            _context.SaveChanges();

         
            return RedirectToAction("Index"); 
        }

    }
    
    }


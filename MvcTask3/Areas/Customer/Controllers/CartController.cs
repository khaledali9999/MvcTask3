using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcTask3.Models;
using MvcTask3.Repos;
using System.Linq.Expressions;

namespace MvcTask3.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Cart> _cartRepository;
        //private readonly IRepository<Promotion> _promotionRepository;
        
        public CartController(UserManager<ApplicationUser> userManager, IRepository<Cart> cartRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            // هنا بنجيب كل عناصر الكارت بتاعة المستخدم الحالي
            var cartItems = await _cartRepository.GetAllAsync(
      c => c.ApplicationUserId == user.Id,
      new List<Expression<Func<Cart, object>>>
      {
        c => c.Movie
      },
      cancellationToken
  );


            return View(cartItems); // نبعت العناصر للـ View
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int count, int movieId, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return NotFound();

            var productInDb = await _cartRepository.GetOneAsync(e => e.ApplicationUserId == user.Id && e.MovieId == movieId);

            if (productInDb is not null)
            {
                productInDb.Count += count;
                await _cartRepository.CommitAsync(cancellationToken);

                TempData["success-notification"] = "Update Product Count to cart successfully";

                return RedirectToAction("Index", "Home");
            }

            await _cartRepository.AddAsync(new()
            {
                MovieId = movieId,
                Count = count,
                ApplicationUserId = user.Id,
                //Price = (await _productRepository.GetOneAsync(e => e.Id == productId)!).Price
            }, cancellationToken: cancellationToken);
            await _cartRepository.CommitAsync(cancellationToken);

            TempData["success"] = "Add Product to cart successfully";

            return RedirectToAction("Index", "Home");
        }

    }
}

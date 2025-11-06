using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MvcTask3.Models;
using MvcTask3.ViewModels;

namespace MvcTask3.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) { 
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            return View(registerVM);

          var result =  await _userManager.CreateAsync(new()
            {
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Email = registerVM.Email,
                UserName = registerVM.UserName,
            }, registerVM.Password);

            if (!result.Succeeded) {
                foreach (var item in result.Errors) {
                    ModelState.AddModelError(string.Empty, item.Code);
                }
                return View(registerVM);
            }
            return RedirectToAction("Login");

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Login(LoginVM loginVM)
        {

            if (!ModelState.IsValid)
                return View(loginVM);
            var user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail)
      ?? await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);


            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid UserName Or Email");
                return View(loginVM);
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe,lockoutOnFailure: true);
            if (!result.Succeeded) {
                if (result.IsLockedOut)
                    ModelState.AddModelError(string.Empty,"too many atemp , try again after 5 min");
                else 
                ModelState.AddModelError(string.Empty, "Invalid UserName Or Email Or Password");
                return View(loginVM);

            }
            return RedirectToAction("Index", "Home", new {area = "Customer" });
        }
       
    }
}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MvcTask3.Models;
using MvcTask3.ViewModels;

namespace MvcTask3.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Authorize]
    public class ProfileController : Controller
    {
        private UserManager<ApplicationUser> _UserManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
           _UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; }

        public async Task<IActionResult> Index()
        {
            var user = await _UserManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var vm = new ViewModels.ApplicationUserVM()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = $"{user.FirstName} {user.LastName}",
                Address = user.Address!,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!
            };

            return View(vm);
        }
        [HttpPost]

        public async Task<IActionResult> UpdateProfile(ApplicationUserVM applicationUserVM)
        {
            var user = await _UserManager.GetUserAsync(User);
            if (user is null)
                return NotFound();

            user.PhoneNumber = applicationUserVM.PhoneNumber;
            user.FirstName = applicationUserVM.FirstName;
            user.LastName = applicationUserVM.LastName;
            user.Address = applicationUserVM.Address;

            await _UserManager.UpdateAsync(user);

            // ✅ إضافة رسالة Toastr
            TempData["Success"] = "Profile updated successfully!";

            return RedirectToAction(nameof(Index));
        }
        public async Task <IActionResult>  UpdatePassword(ApplicationUserVM applicationUserVM)
        {
            var user = await _UserManager.GetUserAsync(User);
            if (user is null)
                return NotFound();
            if (applicationUserVM.CurrentPassword is null || applicationUserVM.NewPassword is null)
            {
                TempData["Error"] = "Must Have A CurrentPassword & NewPassword";
                return RedirectToAction(nameof(Index));
            }
         var result =  await  _UserManager.ChangePasswordAsync( user, applicationUserVM.CurrentPassword, applicationUserVM.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
            TempData["Error"] = string.Join(" ", result.Errors.Select(e => e.Code));


                return RedirectToAction(nameof(Index));
            }
            TempData["Success"] = "Profile updated successfully!";

            return RedirectToAction(nameof(Index));

        }
    }

}

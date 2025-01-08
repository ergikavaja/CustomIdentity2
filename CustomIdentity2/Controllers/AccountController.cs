using CustomIdentity2.Models;
using CustomIdentity2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CustomIdentity2.Controllers
{
    public class AccountController(SignInManager<AppUser> _signInManager) : Controller
    {
        private readonly SignInManager<AppUser> signInManager = _signInManager;
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "invalid login attempt!");
                return View(model);

            }
            return View(model);
        }

    }
}

using CustomIdentity2.Models;
using CustomIdentity2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomIdentity2.Controllers
{
    public class AccountController(SignInManager<AppUser> _signInManager, UserManager<AppUser> _userManager, RoleManager<IdentityRole> roleManager) : Controller
    {
        private readonly SignInManager<AppUser> signInManager = _signInManager;
        private readonly UserManager<AppUser> userManager = _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //_roleManager = roleManager;
        public IActionResult Login()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public class AdminController : Controller
        {
            public IActionResult Index()
            {
                return View();
            }
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


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model, string role)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.Name, Email = model.Email };
                var result = await _userManager.CreateAsync((AppUser)user, model.Password);

                if (result.Succeeded)
                {
                    // Assign the selected role
                    if (!string.IsNullOrEmpty(role) && await _roleManager.RoleExistsAsync(role))
                    {
                        await _userManager.AddToRoleAsync((AppUser)user, role);
                    }

                    // Automatically log the user in after registration
                    await _signInManager.SignInAsync((AppUser)user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    


        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterVM model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        AppUser user = new()
        //        {
        //            Name = model.Name,
        //            UserName = model.Email,
        //            Email = model.Email,
        //        };
        //        var result = await _userManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            //await _signInManager.SignInAsync(user, false);
        //            return RedirectToAction("Employees", "Account");
        //        }
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }
        //    }
        //    return View(model);
        //}


        public async Task<IActionResult> Users()
        {
            List<AppUser> users = await _userManager.Users.ToListAsync();
            return View(users);
        }


        //[HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}

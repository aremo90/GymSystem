using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.AccountViewModel;
using GymSystemDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly Microsoft.AspNetCore.Identity.SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService , SignInManager<ApplicationUser> signInManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }

        #region Login Action

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("InvalidModel", "Invalid Email Or password");
                return View(model);
            }
            var User = _accountService.ValidateUser(model);
            if (User is null)
            {
                ModelState.AddModelError("InvalidModel", "Invalid Email Or password");
                return View(model);
            }

            var Result = _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false).Result;
            
            if (Result.IsNotAllowed)
                ModelState.AddModelError("InvalidModel", "Account Not Allowed"); 
            if (Result.IsLockedOut)
                ModelState.AddModelError("InvalidModel", "Account LockedOut");
            if (Result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View(model);
        }

        #endregion

        #region Logout Action
        [HttpPost]
        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction("Login");
        }

        #endregion

        #region Access Denied

        public ActionResult AccessDenied()
        {
            return View();
        }

        #endregion




    }
}

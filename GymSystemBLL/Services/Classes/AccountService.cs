using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.AccountViewModel;
using GymSystemDAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public ApplicationUser? ValidateUser(LoginViewModel LoginViewModel)
        {
            var User = _userManager.FindByEmailAsync(LoginViewModel.Email).Result;
            if (User is null) return null;
            var IsPassVaild = _userManager.CheckPasswordAsync(User, LoginViewModel.Password).Result;
            return IsPassVaild ? User : null;
        }
    }
}

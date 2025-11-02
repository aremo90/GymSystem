using GymSystemDAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.DataSeed
{
    public class IdentityDbContextSeeding
    {
        public static bool SeedData(RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager)
        {
			try
			{
				var HasUsers = userManager.Users.Any();
				var HasRoles = roleManager.Roles.Any();

				if (HasUsers && HasRoles) return false;

				if (!HasRoles)
				{
					var Roles = new List<IdentityRole>()
					{
						new() {Name = "SuperAdmin" },
						new() {Name = "Admin" }
					};

					foreach (var role in Roles)
					{
						if (!roleManager.RoleExistsAsync(role.Name!).Result)
						{
                            roleManager.CreateAsync(role).Wait();
                        }
                    }
				}

				if (!HasUsers)
				{
					var MainAdmin = new ApplicationUser()
					{
						FirstName = "Hamza",
						LastName = "Ahmed",
						UserName = "HamzaAhmed",
						Email = "HamzaAhmed@gmail.com",
						PhoneNumber = "01155014841",
					};
					userManager.CreateAsync(MainAdmin, "P@ssw0rd").Wait();
					userManager.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();
					var Admin = new ApplicationUser()
					{
						FirstName = "Omar",
						LastName = "Ahmed",
						UserName = "OmarAhmed",
						Email = "OmarAhmed@gmail.com",
						PhoneNumber = "01155014842",
					};
					userManager.CreateAsync(Admin, "P@ssw0rd").Wait();
					userManager.AddToRoleAsync(Admin, "Admin").Wait();
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
        }
    }
}

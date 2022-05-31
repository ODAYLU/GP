using GP.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP
{
    public static class SeedData
    {
        public static void Seed(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
        public static List<long> VsLikedEstate { get; set; }
        public static bool IsPserosalPhoto { get; set; }

        public static long EstateByImage { get; set; }
        public static void SeedUsers(
            UserManager<AppUser> userManager)
        {
            if(userManager.FindByNameAsync("admin@website.com").Result == null)
            {
                var user = new AppUser
                {
                    UserName = "admin@website.com",
                    Email = "admin@website.com",
                    is_active = true,
                };
                var result = userManager.CreateAsync(user ,"a@1234567").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,"Admin").Wait();
                }
            }
        }
        public static void SeedRoles(
            RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Admin",
                };
              var result = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Owner").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Owner",
                };
                roleManager.CreateAsync(role);
            }
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                var role = new IdentityRole
                {
                    Name = "User",
                };
                roleManager.CreateAsync(role);
            }
        }
    }
}

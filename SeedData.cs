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
        public static async Task Seed(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ICategory category,
            IType type,
            IState state,
            ICity city,
            IService service,
            IInformationGen information)
        {
         await   SeedRoles(roleManager);
          await  SeedUsers(userManager);

            if (!category.GetAll().Any())
            {
              
                var cat1 = new Category
                {
                    category = "منزل",
                    ImagePath = "/images/house.jpg",
                };
                var cat2 = new Category
                {
                    category = "شقة",
                    ImagePath = "/images/house.jpg",
                };
                var cat3 = new Category
                {
                    category = "شاليه",
                    ImagePath = "/images/house.jpg",
                };
                var cat4 = new Category
                {
                    category = "أرض",
                    ImagePath = "/images/house.jpg",
                };
           
                await  category.InsertCategory(cat1);
                await category.InsertCategory(cat2);
                await category.InsertCategory(cat3);
                await category.InsertCategory(cat4);
            }

        }
        public static List<long> VsLikedEstate { get; set; }
        public static bool IsPserosalPhoto { get; set; }

        public static long EstateByImage { get; set; }
        public static async Task SeedUsers(
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
        public static async Task SeedRoles(
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
            if (!(await roleManager.RoleExistsAsync("User")))
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

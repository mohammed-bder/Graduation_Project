using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public async static Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            // Seed, if necessary
            if (userManager.Users.Count() == 0)
            {
                var user = new AppUser
                {
                    FullName = "Mohammed Hassan",
                    Email = "Mohammed.Hassan@gmail.com",
                    UserName = "Mohammed.Hassan",
                    PhoneNumber = "01000000000",
                };

                var result = await userManager.CreateAsync(user, "Pa$$w0rd");

                if (result.Succeeded)
                {
                    Console.WriteLine("User created successfully");
                }

            }

        }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Repository.Identity
{
    public static class RoleSeed
    {

        public static async Task RoleSeedAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles =
            {
                "Admin",
                "Doctor",
                "Patient",
                "Secretary",
                "Pharmacist",
            };
            foreach (var role in roles)
            {
                if(! await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

        }
    }
}

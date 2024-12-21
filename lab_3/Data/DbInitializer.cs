using lab_3.Models;
using Microsoft.AspNetCore.Identity;

namespace lab_3.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(UserDbContext userDbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // создаем роли
            string[] roles = { "Manager", "Passenger", "Boss" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // создаем пользователей
            var usersWithRoleBoss = await userManager.GetUsersInRoleAsync("Boss");
            if (!usersWithRoleBoss.Any()) // нету босса
            {
                var stationBoss = new User
                {
                    FirstName = "Василий",
                    LastName = "Николаевич",
                    UserName = "boss1",
                    PhoneNumber = "+375445839393",
                };

                string password = "123456";
                var result = await userManager.CreateAsync(stationBoss, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(stationBoss, "Boss");
                }
            }

            var usersWithRoleManager = await userManager.GetUsersInRoleAsync("Manager");
            if (!usersWithRoleManager.Any()) // нету босса
            {
                var manager = new User
                {
                    FirstName = "Александр",
                    LastName = "Крючков",
                    UserName = "alexandr",
                    PhoneNumber = "+375445839123",
                };

                string password = "123456";
                var result = await userManager.CreateAsync(manager, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(manager, "Manager");
                }
            }

            var usersWithRolePassenger = await userManager.GetUsersInRoleAsync("Passenger");
            if (!usersWithRolePassenger.Any()) // нету босса
            {
                var passenger = new User
                {
                    FirstName = "Максим",
                    LastName = "Богданович",
                    UserName = "maksim",
                    PhoneNumber = "+375445839456",
                };

                string password = "123456";
                var result = await userManager.CreateAsync(passenger, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(passenger, "Passenger");
                }
            }
            {
                // passenger2
                var passenger = new User
                {
                    FirstName = "йдцви",
                    LastName = "дышаргши",
                    UserName = "user",
                    PhoneNumber = "+375445839456",
                };

                string password = "123456";
                var result = await userManager.CreateAsync(passenger, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(passenger, "Passenger");
                }
            }
        }
    }
}

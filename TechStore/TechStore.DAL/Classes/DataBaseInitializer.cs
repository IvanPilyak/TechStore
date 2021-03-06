﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TechStore.DAL.Models;

namespace TechStore.DAL.Classes
{
    public class DataBaseInitializer
    {
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            AppDbContext appContext)
        {
            AppDbContext appDbContext = appContext;
            UnitOfWork.UnitOfWork unitOfWork = new UnitOfWork.UnitOfWork(appDbContext);
            string adminEmail = "administrator123@gmail.com";
            string adminPassword = "Admin123";

            string managerEmail = "manager@manager.com";
            string managerPassword = "Manager123";

            string moderatorEmail = "moderator@moderator.com";
            string moderatorPassword = "Moderator123";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await roleManager.FindByNameAsync("moderator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("moderator"));
            }

            if (await roleManager.FindByNameAsync("customer") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("customer"));
            }

            if (await roleManager.FindByNameAsync("manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("manager"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                ApplicationUser admin = new ApplicationUser { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

            if (await userManager.FindByNameAsync(managerEmail) == null)
            {
                ApplicationUser manager = new ApplicationUser { Email = managerEmail, UserName = managerEmail };
                IdentityResult result = await userManager.CreateAsync(manager, managerPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(manager, "manager");
                }
            }

            if (await userManager.FindByNameAsync(moderatorEmail) == null)
            {
                ApplicationUser moderator = new ApplicationUser { Email = moderatorEmail, UserName = moderatorEmail };
                IdentityResult result = await userManager.CreateAsync(moderator, moderatorPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(moderator, "moderator");
                }
            }

            if (appDbContext != null)
            {
                if (!unitOfWork.Customers.GetAll().Any())
                {
                    await unitOfWork.Customers.Create(
                        new Customer()
                        {
                            Id = 1,
                            FirstName = "Adam",
                            SecondName = "Rowan",
                            Email = "asn@gmail.com"
                        });
                    await unitOfWork.SaveAsync();
                }

                
                if (!unitOfWork.Storages.GetAll().Any())
                { 
                    await unitOfWork.Storages.Create(new Storage() { City = "Lviv", Street = "Horodotska, 17" });
                    await unitOfWork.Storages.Create(new Storage() { City = "Kiev", Street = "Kachalova, 54" });
                }

                if (!unitOfWork.Producers.GetAll().Any())
                {
                    await unitOfWork.Producers.Create(new Producer()
                    {
                        Name = "Impression",
                        Phone = "443230303",
                        Email = "info@impression.ua",
                        WebSite = "https://impression.ua/"
                    });

                    await unitOfWork.SaveAsync();
                }

                //if (unitOfWork.Orders.GetAll().Count() == 0)
                //{
                //    await unitOfWork.Orders.Create(new Order()
                //    {
                //        OrderDate = new DateTime(2017, 7, 12),
                //        Customer = await unitOfWork.Customers.Get(appContext.Customers.First().Id),
                //        OrderStatus = OrderStatus.Cancelled
                //    });

                //    await unitOfWork.SaveAsync();
                //}
            }
        }
    }
}

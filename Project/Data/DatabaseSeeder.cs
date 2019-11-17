using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Data
{
    public class DatabaseSeeder
    {
        private readonly DatabaseContext _ctx;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        public DatabaseSeeder(DatabaseContext ctx,
            IHostingEnvironment hosting,
            UserManager<StoreUser> userManager)
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }
        public async Task Seed()
        {
            _ctx.Database.EnsureCreated();

            var user =await _userManager.FindByEmailAsync("masum@gmail.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    firstName = "Masum",
                    lastName = "Billah",
                    UserName = "masum@gmail.com",
                    Email = "masum@gmail.com"
                };
                var results = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if(results != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Failed to create default user");
                }
            }

            if (!_ctx.Products.Any())
            {
                //Need to create sample data
                var filepath = Path.Combine(_hosting.ContentRootPath,"Data/art.json");
                var json = File.ReadAllText(filepath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                _ctx.Products.AddRange(products);
                var order = new Order()
                {
                    OrderDate = DateTime.Now,
                    OrderNumber = "123456",
                    User=user,
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                        Product=products.First(),
                        Quentity=5,
                        UnitPrice=products.First().Price
                        }
                    }
                };
                _ctx.Orders.Add(order);
                _ctx.SaveChanges();
            }
        }
    }
}

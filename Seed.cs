using Learning.Data;
using Learning.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.WebSockets;
using Learning.Interfaces.BusinessLogic;
using Learning.BusinessLogic;
using Learning.Services;

namespace Learning
{
    public class Seed
    {
        private readonly DataContext context;
        public Seed(DataContext context)
        {
            this.context = context;
        }
        public async Task SeedDataContextAsync()
        {
            await SeedUsers();
            await SeedProducts();
            await SeedOrders();
        }

        private async Task SeedUsers()
        {
            var userBL = new UserBusinessLogic(context);
            var user = context.Roles.FirstOrDefault(r => r.NormalizedName == "USER");
            var producer = context.Roles.FirstOrDefault(r => r.NormalizedName == "PRODUCER");
            var admin = context.Roles.FirstOrDefault(r => r.NormalizedName == "ADMIN");
            var encription = new PasswordEncription("admin123");
            await userBL.CreateUser(new User
            { 
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                FirstName = "admin",
                LastName = "admin",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                TwoFactorEnabled = false,
                DateCreated = DateTime.Now,
                CurrentSalt = encription.GetSalt(),
                CurrentHash = encription.GetHash(),
                Passwords = new List<Password>() { new Password { Hash = encription.GetHash(), Salt = encription.GetSalt() } },
                Adresses = new List<Adress>()
                {
                    new Adress
                    {
                        City = "Cluj-Napoca",
                        Street = "Constanta",
                        Number = "12",
                        State = "Cluj",
                        Country = "Romania",
                        Phone = "0757111111",
                    }
                },
                RoleUser = new List<RoleUser>()
                {
                    new RoleUser {Role = admin},
                    new RoleUser {Role = user}
                }
            });
            encription = new PasswordEncription("consumer123");
            await userBL.CreateUser(new User
            {
                UserName = "consumer",
                NormalizedUserName = "CONSUMER",
                FirstName = "consumer",
                LastName = "consumer",
                Email = "consumer@consumer.com",
                NormalizedEmail = "CONSUMER@CONSUMER.COM",
                EmailConfirmed = true,
                TwoFactorEnabled = false,
                DateCreated = DateTime.Now,
                CurrentSalt = encription.GetSalt(),
                CurrentHash = encription.GetHash(),
                Passwords = new List<Password>() { new Password { Hash = encription.GetHash(), Salt = encription.GetSalt() } },
                Adresses = new List<Adress>()
                    {
                        new Adress
                        {
                            City = "Cluj-Napoca",
                            Street = "Ciocarliei",
                            Number = "40",
                            State = "Cluj",
                            Country = "Romania",
                            Phone = "0757222222"
                        }
                    },
                RoleUser = new List<RoleUser>()
                {
                    new RoleUser {Role = user}
                }
            });
            encription = new PasswordEncription("producer123");
            await userBL.CreateUser(new User
            {
                UserName = "producer",
                NormalizedUserName = "PRODUCER",
                FirstName = "producer",
                LastName = "producer",
                Email = "producer@producer.com",
                NormalizedEmail = "PRODUCER@PRODUCER.COM",
                EmailConfirmed = true,
                TwoFactorEnabled = false,
                DateCreated = DateTime.Now,
                CurrentSalt = encription.GetSalt(),
                CurrentHash = encription.GetHash(),
                Passwords = new List<Password>() { new Password { Hash = encription.GetHash(), Salt = encription.GetSalt() } },
                Adresses = new List<Adress>()
                    {
                        new Adress
                        {
                            City = "Ocna Mures",
                            Street = "Crisan",
                            Number = "10",
                            State = "Alba",
                            Country = "Romania",
                            Phone = "0757333333"
                        }
                    },
                RoleUser = new List<RoleUser>()
                {
                    new RoleUser {Role = producer},
                    new RoleUser {Role = user}
                }
            });
        }

        private async Task SeedProducts()
        {
            var productBL = new ProductBusinessLogic(context);
            var user = context.Users.FirstOrDefault(u => u.NormalizedUserName == "PRODUCER");
            await productBL.CreateProduct(new Product
            {
                Name = "Samsung Galaxy",
                Type = "Phone",
                Description = "Best Android Phone",
                Price = 1000,
                Stock = 100,
                User = user
            });
            await productBL.CreateProduct(new Product
            {
                Name = "Jabbar Elite",
                Type = "Headphones",
                Description = "Best Head Phones",
                Price = 100,
                Stock = 500,
                User = user
            });
            await productBL.CreateProduct(new Product
            {
                Name = "harry potter",
                Type = "book",
                Description = "Best Seller Fantasy Book",
                Price = 20,
                Stock = 50,
                User = user
            });
        }

        private async Task SeedOrders()
        {
            var orderBL = new OrderBusinessLogic(context);
            var adress = context.Users.FirstOrDefault(u => u.NormalizedUserName == "CONSUMER").Adresses.FirstOrDefault();
            var product1 = context.Products.FirstOrDefault(p => p.Name == "harry potter");
            var product2 = context.Products.FirstOrDefault(p => p.Name == "Jabbar Elite");
            var product3 = context.Products.FirstOrDefault(p => p.Name == "Samsung Galaxy");
            var order1 = new Order
            {
                OrderNumber = "order_1",
                DateCreated = DateTime.Now,
                Status = "Delivered",
                Transport = "cargus",
                Cost = 20,
                Devivered = 1,
                Total = 1,
                Adress = adress,
                ProductOrders = new List<ProductOrder> { new ProductOrder { Product = product1, Status = "Delivered" } }
            };
            await orderBL.CreateOrder(order1);
            var order2 = new Order
            {
                OrderNumber = "order_2",
                DateCreated = DateTime.Now,
                Status = "Delivered",
                Transport = "easybox",
                Devivered = 2,
                Total = 2,
                Cost = 1100,
                Adress = adress,
                ProductOrders = new List<ProductOrder> { new ProductOrder { Product = product2, Status = "Delivered" }, new ProductOrder { Product = product3, Status = "Delivered" } }
            };
            await orderBL.CreateOrder(order2);
        }
    }
}

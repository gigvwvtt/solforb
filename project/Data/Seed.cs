using Microsoft.EntityFrameworkCore;
using project.Models;

namespace project.Data;

public static class Seed
{
    public static void SeedData(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
            context.Database.Migrate();
            context.Database.EnsureCreated();

            if (!context.Providers.Any())
            {
                context.Providers.AddRange(new List<Provider>()
                {
                    new Provider() { Name = "Поставщик 1" },
                    new Provider() { Name = "Поставщик 2" },
                    new Provider() { Name = "Поставщик 3" },
                    new Provider() { Name = "Поставщик 4" },
                    new Provider() { Name = "Поставщик 5" },
                    new Provider() { Name = "Поставщик 6" },
                    new Provider() { Name = "Поставщик 7" },
                });
                context.SaveChanges();
            }

            if (!context.Orders.Any())
            {
                context.Orders.AddRange(new List<Order>()
                {
                    new Order()
                    {
                        Date = DateTime.Now,
                        Number = "1",
                        ProviderId = 1,
                    },
                    new Order()
                    {
                        Date = DateTime.Now.AddDays(-2),
                        Number = "2",
                        ProviderId = 2,
                    },
                    new Order()
                    {
                        Date = DateTime.Now.AddDays(-1),
                        Number = "3",
                        ProviderId = 1,
                    },
                    new Order()
                    {
                        Date = DateTime.Now.AddDays(-1),
                        Number = "4",
                        ProviderId = 2,
                    },
                    new Order()
                    {
                        Date = DateTime.Now.AddDays(-5),
                        Number = "ACV2",
                        ProviderId = 1,
                    },
                    new Order()
                    {
                        Date = DateTime.Now.AddDays(-4),
                        Number = "REAS",
                        ProviderId = 3,
                    },
                    new Order()
                    {
                        Date = DateTime.Now.AddDays(-34),
                        Number = "BNN",
                        ProviderId = 5,
                    },
                    new Order()
                    {
                        Date = DateTime.Now.AddDays(-34),
                        Number = "AZ1",
                        ProviderId = 6
                    },
                });
                context.SaveChanges();
            }

            if (!context.OrderItems.Any())
            {
                context.OrderItems.AddRange(new List<OrderItem>()
                {
                    new OrderItem()
                    {
                        Name = "Товар 1",
                        Unit = "Модель B",
                        OrderId = 1,
                        Quantity = 2,
                    },
                    new OrderItem()
                    {
                        Name = "Товар 2",
                        Unit = "Модель C",
                        OrderId = 2,
                        Quantity = 1,
                    },
                    new OrderItem()
                    {
                        Name = "Товар 5",
                        Unit = "Модель X",
                        OrderId = 1,
                        Quantity = 1,
                    },
                    new OrderItem()
                    {
                        Name = "Товар 123",
                        Unit = "Модель SAX",
                        OrderId = 2,
                        Quantity = 1,
                    },
                    new OrderItem()
                    {
                        Name = "Товар 65",
                        Unit = "Модель SFX",
                        OrderId = 4,
                        Quantity = 4,
                    },
                    new OrderItem()
                    {
                        Name = "Товар VB",
                        Unit = "Модель XZ",
                        OrderId = 3,
                        Quantity = 2,
                    },
                    new OrderItem()
                    {
                        Name = "Товар M",
                        Unit = "Модель JY",
                        OrderId = 3,
                        Quantity = 23,
                    },
                    new OrderItem()
                    {
                        Name = "Товар V5",
                        Unit = "Модель JxP",
                        OrderId = 5,
                        Quantity = 6,
                    },
                });
                context.SaveChanges();
            }
        }
    }
}
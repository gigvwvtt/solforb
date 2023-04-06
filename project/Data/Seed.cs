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

            if (!context.Orders.Any())
            {
                context.Orders.AddRange(new List<Order>()
                {
                    new Order()
                    {
                        Date = DateTime.Now,
                        Number = "1",
                        Provider = new Provider() { Name = "Provider 1" },
                    },
                    new Order()
                    {
                        Date = DateTime.Now.AddDays(-2),
                        Number = "2",
                        Provider = new Provider() { Name = "Provider 2" },
                    },
                    new Order()
                    {
                        Date = DateTime.Now.AddDays(-1),
                        Number = "3",
                        ProviderId = 2,
                    },
                    new Order()
                    {
                        Date = DateTime.Now.AddDays(-1),
                        Number = "4",
                        ProviderId = 1,
                    },
                });
                context.SaveChanges();
            }
        }
    }
}
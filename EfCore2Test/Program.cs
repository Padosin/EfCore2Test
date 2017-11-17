using EfCore2Test.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EfCore2Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var servicesCollection = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

            servicesCollection.AddDbContext<TestDbContext>();

            servicesCollection.AddScoped<FilterOption>();
            servicesCollection.AddScoped<FilterProvider>();

            var provider = servicesCollection.BuildServiceProvider();

            var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

            
            using (var scope = scopeFactory.CreateScope())
            {
                var db1 = scope.ServiceProvider.GetRequiredService<TestDbContext>();
                db1.Database.EnsureDeleted();
                db1.Database.EnsureCreated();

                db1.Teams.Add(new Team() { IsDelete = false, Name = "Team1" });
                db1.Teams.Add(new Team() { IsDelete = true, Name = "Team2" });

                db1.SaveChanges();
            }

            using (var scope = scopeFactory.CreateScope())
            {

                var option = scope.ServiceProvider.GetRequiredService<FilterOption>();
                option.IsSoftDeleteEnabled = true;

                var db1 = scope.ServiceProvider.GetRequiredService<TestDbContext>();

                var teams = db1.Teams.ToList();
            }

            using (var scope2 = scopeFactory.CreateScope())
            {
                var option = scope2.ServiceProvider.GetRequiredService<FilterOption>();
                option.IsSoftDeleteEnabled = false;

                var db2 = scope2.ServiceProvider.GetRequiredService<TestDbContext>();

                var teams = db2.Teams.ToList();
            }

        }
    }
}

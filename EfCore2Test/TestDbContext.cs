using EfCore2Test.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EfCore2Test
{
    public class TestDbContext : DbContext
    {
        public FilterProvider FilterProvider { get; set; }

        public DbSet<Team> Teams { get; set; }

        public TestDbContext(FilterProvider filterProvider)
            : base()
        {
            FilterProvider = filterProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Need to replace the factory to not used cached model with previous filters option.
            optionsBuilder.ReplaceService<IModelCacheKeyFactory, TestModelCacheKeyFactory>();

            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS; Database=QueryFiltersTest; Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            var models = modelBuilder.Model.GetEntityTypes();

            foreach (var model in models)
            {
                FilterProvider.InitializeFilters(modelBuilder, model.ClrType);
            }
        }
        
    }
}

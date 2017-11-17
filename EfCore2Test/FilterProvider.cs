using EfCore2Test.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EfCore2Test
{
    public class FilterProvider
    {
        public FilterOption Option { get; }

        public FilterProvider(FilterOption option)
        {
            Option = option;
        }


        public static Expression<Func<TEntity, bool>> GetSoftDeleteExpression<TEntity>()
            where TEntity : class, ISoftDelete
        {
            return (p => p.IsDelete != true);
        }


        public void InitializeFilters(ModelBuilder modelBuilder, Type modelType)
        {
            var method = typeof(FilterProvider)
                .GetMethod(nameof(FilterProvider.SetFilters))
                .MakeGenericMethod(modelType);

            method.Invoke(null, new object[] { modelBuilder, Option });
        }

        public static void SetFilters<TEntity>(ModelBuilder modelBuilder, FilterOption option)
            where TEntity : class
        {
            var modelType = typeof(TEntity);
            var interfaces = modelType.GetInterfaces();

            Expression<Func<TEntity, bool>> softDelExpression = null;

            if (option.IsSoftDeleteEnabled && interfaces.Contains(typeof(ISoftDelete)))
            {
                var method = typeof(FilterProvider)
                    .GetMethod(nameof(FilterProvider.GetSoftDeleteExpression))
                    .MakeGenericMethod(modelType);

                softDelExpression = (Expression<Func<TEntity, bool>>)method.Invoke(null, new object[] { });
            }

            // Some another filters may be here...

            modelBuilder.Entity<TEntity>().HasQueryFilter(
                    softDelExpression);

        }
    }
}

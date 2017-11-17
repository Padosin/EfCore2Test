using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfCore2Test
{
    public class TestModelCacheKeyFactory : ModelCacheKeyFactory, IModelCacheKeyFactory
    {
        public TestModelCacheKeyFactory(ModelCacheKeyFactoryDependencies dependencies)
            : base(dependencies)
        {
        }

        public override object Create(DbContext context) => new TestModelCacheKey(context);
    }
}

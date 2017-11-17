using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfCore2Test
{
    internal class TestModelCacheKey
    {

        private readonly Type _dbContextType;
        private readonly bool _filtersOptionParams;
        public TestModelCacheKey(DbContext context)
        {
            _dbContextType = context.GetType();
            _filtersOptionParams = ((TestDbContext)context).FilterProvider.Option.IsSoftDeleteEnabled;
        }

        protected virtual bool Equals(TestModelCacheKey other) => _filtersOptionParams == other._filtersOptionParams && _dbContextType == other._dbContextType;

        public override bool Equals(object obj)
        {
            var otherAsKey = obj as TestModelCacheKey;
            return (otherAsKey != null) && Equals(otherAsKey);
        }

        public override int GetHashCode()
        {
            if (_dbContextType == null)
            {
                return 0;
            }
            else
            {
                return _dbContextType.GetHashCode() ^ _filtersOptionParams.GetHashCode();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EfCore2Test.Model
{
    public interface ISoftDelete
    {
        bool IsDelete { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L6.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        PlayerAdminContext Get();
    }
}

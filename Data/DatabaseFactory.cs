using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L6.Data.Infrastructure
{
public class DatabaseFactory : Disposable, IDatabaseFactory
{
    private PlayerAdminContext dataContext;
    public PlayerAdminContext Get()
    {
        return dataContext ?? (dataContext = new PlayerAdminContext());
    }
    protected override void DisposeCore()
    {
        if (dataContext != null)
            dataContext.Dispose();
    }
}
}

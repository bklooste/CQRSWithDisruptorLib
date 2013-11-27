using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L6.Data.Infrastructure
{
    /// <summary>
    /// NOte this must all be the same context !...so do not use from a global container...
    /// </summary>
    public interface IUnitOfWork
    {
        void Commit();
    }

    public interface IUnitOfWorkProvider
    {
        IUnitOfWork UnitOfWork { get; } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.Commands
{

    public interface ICommandHandler { }

    public interface ICommandHandler<T> : ICommandHandler where T : Command
    {
        void Handle(T args);
        //  IDomain Context { get; } 


    } 
}

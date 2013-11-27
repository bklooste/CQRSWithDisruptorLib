using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.Events
{
    public interface IHandles {


    } 

    public interface IHandles<T> : IHandles where T:DomainEvent
    {
        void Handle(T message);

    }
}

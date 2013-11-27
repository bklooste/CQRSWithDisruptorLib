using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.Events
{

    public interface IStartedByEvent 
    {
        void Start(DomainEvent val); 
    }

    public interface IStartedByEvent<T> : IStartedByEvent where T : DomainEvent
    {

        void Start(T val);
    }
}

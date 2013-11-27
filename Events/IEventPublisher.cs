using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.Events
{

    public interface IEventPublisher
    {
        void Publish<T>(T @event) where T : DomainEvent;
        void Publish(DomainEvent @event); // late binding  
    }

}

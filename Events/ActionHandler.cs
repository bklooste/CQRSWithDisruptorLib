using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L6.Infrastructure.Events
{
    internal class ActionHandler<T> :IHandles<DomainEvent> where T:DomainEvent
    {
        Action<T> action;

        internal ActionHandler(Action<T> action)
        {
            this.action = action;
        }

        public void Handle(T message)
        {
            action(message); 
        }

        public void Handle(DomainEvent message)
        {
            action( (T) message );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.Events
{
    public class EventHandler<T> : IHandles<T> where T:DomainEvent
    {

          Action<T>  callback;
          public EventHandler(Action<T> callback)
        {
            this.callback =  callback;

        }

        public void Handle(T message)
        {
             callback(message);
        }

        public void Handle(DomainEvent message)
        {
          callback(message as T );
        }
    }
}

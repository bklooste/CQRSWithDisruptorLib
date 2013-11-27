using L6.Infrastructure.Commands;
using L6.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.Util
{
    public class ActionCommandPublisher :ICommandPublisher  //IEventPublisher 
    {
        Action<Command> process;

        public ActionCommandPublisher(Action<Command> process)
        {
            this.process = process;
        }

        public void Publish<T>(T args) where T : Command
        {
            process(args); 

        }
    }

    public class ActionEventPublisher : IEventPublisher  //IEventPublisher 
    {
        Action<DomainEvent> process;

        public ActionEventPublisher(Action<DomainEvent> process)
        {
            this.process = process;
        }

    
        public void Publish(DomainEvent @event)
        {
            process(@event);
        }

        public void Publish<T>(T @event) where T : DomainEvent
        {
            process(@event);
        }
    }
}

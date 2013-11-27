using L6.Infrastructure.Commands;
using L6.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.Domain
{
    public abstract class  EventProcessor : IHandles// experimental !
    {
        ICommandPublisher commandBus;

        protected EventProcessor(ICommandPublisher commandBus)
        {
            this.commandBus = commandBus;
        }


        protected void Fire(Command command)
        {
            commandBus.Publish(command);
        }

    }
}

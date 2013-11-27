using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disruptor;
using L6.Infrastructure.Commands;


namespace L6.Infrastructure.Disruptor
{
    public sealed class DisruptorCommandEventHandler : IEventHandler<CommandHolder>
    {
        ICommandPublisher inDomainEventHandler;

        public DisruptorCommandEventHandler(ICommandPublisher inDomainEventHandler)
        {
            this.inDomainEventHandler = inDomainEventHandler;
        }





        void IEventHandler<CommandHolder>.OnNext(CommandHolder data, long sequence, bool endOfBatch)
        {
            inDomainEventHandler.Publish(data.Value); // FIXME consider if we should have different ones...eg basic events or characterEvents

        }
    }


    //Fixme seperate file
    public sealed class DisruptorActionCommandEventHandler<T> : IEventHandler<T>
    {
        Action<T> action;

        public DisruptorActionCommandEventHandler(Action<T> action)
        {
            this.action = action ;
        }





        void IEventHandler<T>.OnNext(T data, long sequence, bool endOfBatch)
        {
            action(data); 
            //inDomainEventHandler.Publish(data.Value); // FIXME consider if we should have different ones...eg basic events or characterEvents

        }
    }



}

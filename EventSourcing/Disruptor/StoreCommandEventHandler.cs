using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Disruptor;
using L6.Infrastructure.Commands;
using L6.Infrastructure.Disruptor;
using L6.Infrastructure.Util;


namespace L6.Infrastructure.EventSourcing.Disruptor
{
    public sealed class StoreCommandEventHandler : IEventHandler<CommandHolder>
    {
        EventStoreCommandHandler commandHandler;

        public StoreCommandEventHandler(ICommandHandler handler)
        {
            Ensure.NotNull(handler, "handler");
            commandHandler = handler as EventStoreCommandHandler;
        }

        


        //FIXME dynamic dispatch gets a duplicate overload
        void IEventHandler<CommandHolder>.OnNext(CommandHolder data, long sequence, bool endOfBatch)
        {
            //ThreadPool - NO need we call everything on 1 thread and handle call backs via the pool 


            if ( data.Value is SaveEventsStoreCommand)
                commandHandler.Handle((SaveEventsStoreCommand)data.Value);

            if (data.Value is GetEventsForAggregateStoreCommand)
                commandHandler.Handle((GetEventsForAggregateStoreCommand)data.Value);

            if (data.Value is GetHighestIdForTypeStoreCommand)
                commandHandler.Handle((GetHighestIdForTypeStoreCommand)data.Value); 

        }
    }
}

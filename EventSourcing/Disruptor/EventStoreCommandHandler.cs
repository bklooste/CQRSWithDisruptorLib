using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L6.Infrastructure.Commands;
using L6.Infrastructure.Events.EventStore;
using L6.Infrastructure.Util;
using System.Diagnostics;

namespace L6.Infrastructure.EventSourcing.Disruptor
{
    public class EventStoreCommandHandler : ICommandHandler,
          ICommandHandler<SaveEventsStoreCommand>,
          ICommandHandler<GetEventsForAggregateStoreCommand>,
          ICommandHandler<GetHighestIdForTypeStoreCommand>
          //        ICommandHandler<GetEventsForTypeStoreCommand>,
          //ICommandHandler<GetEventsByTypeAndDateAggregateStoreCommand>
    {
        IEventStore store;
        // push in SQL store 

        //we could do the async here but we will push it into the store..
        public EventStoreCommandHandler(IEventStore store)
        {
            this.store = store;
        }

        public void Handle(SaveEventsStoreCommand args)
        {
            try
            {
                store.SaveEvents( args.events, args.expectedVersion);
                if ( args.success != null) 
                    args.success(args.events.First().SourceAggregateRootId);
            }
            catch (Exception failure)
            {
                Debug.WriteLine(failure.ToString());
                if ( args.failure != null)
                    args.failure(failure);
            }
        }

        public void Handle(GetEventsForAggregateStoreCommand args)
        {
            try
            {
                var result = store.GetEventsForObject(args.AggregateId , args.type);
                args.callbackevents(result);
            }
            catch (Exception failure)
            {
                Debug.WriteLine(failure.ToString());
                if (args.failure != null)
                args.failure(failure); 
            }
        }

        public void Handle(GetHighestIdForTypeStoreCommand args)
        {
            try
            {
                var result = store.GetHighestIdForType(args.type);
                args.callbackevent(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetHighestGuidForTypeStoreCommand failed" + ex); 
                //if ( args.failure != null)
                //args.failure(failure);
            }
        }

        //public void Handle(GetEventsByTypeAndDateAggregateStoreCommand args)
        //{
        //    try
        //    {
        //        var result = store.GetEventsByEventType(args.type, args.startDate , args.endDate );
        //        args.callbackevents(result);
        //    }
        //    catch (Exception failure)
        //    {
        //        //args.failure(failure);
        //    }
        //}

        //public void Handle(GetEventsForTypeStoreCommand args)
        //{
        //    try
        //    {
        //        var result = store.GetEventsByEventType(args.type);
        //        args.callbackevents(result);
        //    }
        //    catch (Exception failure)
        //    {
        //      //  args.failure(failure);
        //    }
        //}

        public void HandleDispatch(Command command)
        {
            Handle ((dynamic) command);
        }
    }




}

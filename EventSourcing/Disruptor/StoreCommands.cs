using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L6.Infrastructure.Commands;
using L6.Infrastructure.Domain;
using L6.Infrastructure.Events;
using L6.Infrastructure.Util;

namespace L6.Infrastructure.EventSourcing.Disruptor
{
    public delegate void StoreCommandAction<in T>(T obj); // should use  commands so use CommandAction 

    public class SaveEventsStoreCommand : Command
    {
        //public int AggregateId { get; set; }
        public IEnumerable<DomainEvent> events { get; private  set; }
        // FIXME use different commands if we use this
        public int expectedVersion { get;  private  set; }   // invalid for multiple aggregates
        public StoreCommandAction<int> success { get;private  set; }
        public StoreCommandAction<Exception> failure { get; private set; }

        public SaveEventsStoreCommand(IEnumerable<DomainEvent> events, int expectedVersion, StoreCommandAction<int> success = null, StoreCommandAction<Exception> failure = null)
        {
            Ensure.NotNull<IEnumerable<DomainEvent>>(events, "events");
            if (events.Count() == 0)
                throw new InvalidOperationException("no events");

            //check duplicates
#if DEBUG
            foreach ( var @event in events)
                if (events.Where( x=> x.SourceAggregateRootId == @event.SourceAggregateRootId 
                    && x.SourceAggregateRootType == @event.SourceAggregateRootType
                    && x.Sequence == @event.Sequence).Count() > 1  )
                         throw new InvalidOperationException("event sequence strange");
#endif

            this.events = events;
            this.expectedVersion = expectedVersion;
            this.success = success;
            this.failure = failure;
        }
   //     public string type { get; set; }
    }

    public class GetEventsForAggregateStoreCommand : Command
    {
        public int AggregateId { get; set; }
        public string type { get; set; }

        public StoreCommandAction<IEnumerable<DomainEvent>> callbackevents { get; set; } // must be returned via a command
        public StoreCommandAction<Exception> failure { get; set; }
    }
    // a bit leaky ! 
    public class GetHighestIdForTypeStoreCommand : Command
    {
        public string type { get; set; }
        public StoreCommandAction<int> callbackevent { get; set; } // must be returned via a command
    }

    // warning could be a LOT of data
    public class GetEventsForTypeStoreCommand : Command
    {
        public string type { get; set; }
        internal StoreCommandAction<IDictionary<int, IEnumerable<DomainEvent>>> callbackevents { get; set; } // must be returned via a command
    }


    public class GetEventsByTypeAndDateAggregateStoreCommand : Command
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string type { get; set; }
        public StoreCommandAction<IDictionary<int, IEnumerable<DomainEvent>>> callbackevents { get; set; } // must be returned via a command
    }



}

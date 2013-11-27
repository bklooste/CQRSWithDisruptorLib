using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.Events.EventStore
{


    public interface IEventStore
    {
        void SaveEvents(IEnumerable<DomainEvent> events);  // multiple aggregates
        void SaveEvents(IEnumerable<DomainEvent> events, int expectedVersion);  // one entity  
        IList<DomainEvent> GetEventsForObject(int aggregateId , string type );
        int GetHighestIdForType(string type);

    }


    public interface IEventStoreExtended : IEventStore
    {
        IDictionary<int, IEnumerable<DomainEvent>> GetEventsForAggregateType(int aggregateTypeId);  // prefetch helper
        // eg create snap shot etc
        IDictionary<int, IEnumerable<DomainEvent>> GetEventsByEventType(string domainEventType);
        IDictionary<int, IEnumerable<DomainEvent>> GetEventsByEventType(string domainEventType, int aggregateRootId);
        IDictionary<int, IEnumerable<DomainEvent>> GetEventsByEventType(string domainEventType, DateTime startDate, DateTime endDate);

  
    }

   
}

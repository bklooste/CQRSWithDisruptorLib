using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.Events
{
    // base domain event

    /// <summary>
    /// Sequence ,  the sequence of events for AR
    /// Version , is the version of the message normally via appending v2 .. vn to the type.
    /// 
    /// </summary>
    public class DomainEvent //: Message
    {
        public int Sequence;
        public int  SourceAggregateRootId;
        public string SourceAggregateRootType ;


        public int domainId { get; set; }
    }

    public interface IDomainEventHandler
    {
        //DomainEvent GetEventHandled();
        //void Handle(DomainEvent args);
    }

    public interface IDomainEventHandler<T> : IDomainEventHandler where T : DomainEvent 
    {
        void Handle(T args);
    }
}

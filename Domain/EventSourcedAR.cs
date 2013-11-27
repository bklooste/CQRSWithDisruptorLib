using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L6.Infrastructure.Events;
using L6.Infrastructure.Util;
using System.Diagnostics;

namespace L6.Infrastructure.Domain
{

    // an Entity root
    public abstract class EventSourcedAR<T> : EventSourced , IAggregateRoot<T>
    {
        protected int id; 

        protected abstract override void Dispatch(DomainEvent @event);

        public int AggregateRootId
        {
            get { return id; }
        }

        public void SetId(int id)
        {
            if (this.id != 0)
                Debugger.Break(); 
         //   Debug.WriteLine(this.GetType().Name + " agg set to" + id); 
            this.id = id;
        }

        protected new void Update(DomainEvent @event)
        {
            @event.SourceAggregateRootId = id;

            base.Update(@event); 
        }

    }
}

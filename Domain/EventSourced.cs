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
        /// <summary>
        /// Child classes must Must implement each event as Apply( EVENT) and dispacther
        /// 
        /// Note we dont need to use event BUS
        /// </summary>
        public abstract class EventSourced
        {
            private readonly List<DomainEvent> _changes = new List<DomainEvent>();

            private int sequence = 0;

            public int Sequence
            {
                get { return sequence; }
                protected set { sequence = value; }
            }


            /// <summary>
            /// repository saves and on success marks as commited. 
            /// </summary>
            /// <returns></returns>
            public IEnumerable<DomainEvent> GetUncommittedChanges()
            {
                if (_changes.Count() == 0)
                    Debugger.Break(); 
                return _changes;
            }

            public void MarkChangesAsCommitted()
            {
                _changes.Clear();
            }

            public void MarkChangesAsCommitted(int sequence)
            {
                MarkChangesAsCommitted();
                Sequence = sequence;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="history"> A list of orderered events </param>
            public virtual void LoadFromHistoricalEvents(IEnumerable<DomainEvent> history)
            {

                if (history.Count() == 0) return;

                foreach (var e in history)
                {
                    Dispatch(e);
                    if (sequence > e.Sequence)
                        throw new ArgumentException("Out of order history");
                    sequence = e.Sequence; 
                }
            }


            protected void Update(DomainEvent @event)
            {

                @event.SourceAggregateRootType = this.GetType().ToString();

                if (@event.Sequence != 0)
                    throw new ArgumentException("Invalid event sequence");
             //   @event.Sequence = this.Sequence + 1;

                //FIXME we could  log domaine errors  here
                Dispatch(@event); // will throw with errors 
                _changes.Add(@event);

                // update after changes success!
           //     this.Sequence = @event.Sequence;
            }
          
            //FIXME re-evaluate using a static type to handler dictionary and just pass in an instance when the aggregate is created ...
            // actually we should use a reflecgtion based on here which can be overriden by a custom dispatcher if performance requires it. 
             protected abstract void Dispatch(DomainEvent @event);
         
        }



    }


using L6.Infrastructure.Events;
using L6.Infrastructure.Events.EventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L6.Infrastructure.Commands;
using L6.Infrastructure.Domain;
using L6.Infrastructure.Util;

namespace L6.Infrastructure.EventSourcing.Disruptor
{

    // talks to event store behind a Disruptor



    public class CacheRepository<T> : IDomainRepository<T> 
        where T :  EventSourced , IAggregateRoot<T> , new()
    {
        static int currentId; // use Interloc.increment if you want thread safety


        int cacheSize = 1024; 
        ICommandPublisher store;
        IDictionary<int, TimeOutHolder> cache = new Dictionary<int, TimeOutHolder>();

        long requestCountTimeStamp;  // timestamp 

        event EventHandler completed;
        public event EventHandler CompletedSetup { add { completed += value; } remove { completed -= value; } }
        protected void OnCompleted(Object sender, EventArgs args)
        {
            if (completed != null)
                completed(sender, args);
        }


        public class TimeOutHolder
        {
            private long requestCountTimeStamp;
            private T obj;

            public TimeOutHolder(long requestCountTimeStamp, T obj)
            {
                // TODO: Complete member initialization
                this.requestCountTimeStamp = requestCountTimeStamp;
                this.obj = obj;
            }
            public long LastUsed { get; set; }
            public T Aggregate { get; set; }
        }


        //static DisruptorCacheRepository()
        //{
        //    //poker

        //}

        /// <summary>
        /// count is very important to get right as new identities are issued based on this.
        /// 
        /// we could load all object and have the count but this way we only show objects used and more synergy 
        /// with a expiry cache implementation
        /// </summary>
        /// <param name="store"></param>
        /// <param name="Count"></param>
        public CacheRepository(ICommandPublisher store , int cacheSize = 1024)
        {
            this.store = store;
            this.cacheSize = cacheSize; 
        }

        public void Init(Object data )
        {
            if (currentId != 0)
            {
                OnCompleted(this, null); 
                return;
            }

            store.Publish(new GetHighestIdForTypeStoreCommand()
            {
                type = typeof(T).ToString() ,
                callbackevent =  new StoreCommandAction<int>( x=> LoadCount(x) )
            });
        }

        private void LoadCount(int  value)
        {
            currentId = value;
            OnCompleted(this, null); 
        }

        public void GetById(int id, Action<T> success, System.Action<Exception> failure)
        {
            T cacheValue = LoadCache(id);
            if (cacheValue != null)
                success.Invoke(cacheValue);

            store.Publish(new GetEventsForAggregateStoreCommand()
            {
                AggregateId = id,
                callbackevents = new StoreCommandAction<IEnumerable<DomainEvent>>(x => GetByIdCallback(x, success)),
                failure = new StoreCommandAction<Exception>(x => failure.Invoke(x))
            });
        }



        void GetByIdCallback(IEnumerable<DomainEvent> events, Action<T> success)
        {
            var obj = CreateAndLoadEvents(events);
            AddCache(obj);
            success.Invoke(obj);
        }

        private T CreateAndLoadEvents(IEnumerable<DomainEvent> events)
        {
            var obj = new T();
            obj.LoadFromHistoricalEvents(events);
            return obj;
        }

        T LoadCache(int id)
        {
            requestCountTimeStamp++;
            TimeOutHolder keyValue = null;
            if (cache.TryGetValue(id, out keyValue))
            {
                keyValue.LastUsed = requestCountTimeStamp; //we have a request update the time
                return keyValue.Aggregate;
            }
            else
                return null;
        }

      


        void AddCache(T obj)
        {
            //   EnsureCache(obj.Id); // FAULTY ! 
            cache.Add(obj.AggregateRootId, new TimeOutHolder(requestCountTimeStamp, obj));
            
            
            if ( cache.Count > cacheSize)  // evict old 
            {
               var valsToremove =  cache.Values.OrderBy(x => x.LastUsed).Take(cacheSize/10);
                
                foreach ( var item in valsToremove)
                    cache.Remove(item.Aggregate.AggregateRootId);
            }
        }


        //meaningless as Repository has no state
        public bool TryGet(int id, out T val)
        {
            val = LoadCache(id);
            if (val == null)
                return false;
            else
                return true;
        }

        //meaningless as Repository cant hold all values
        //FIXME hold all for a while
        public void PrefetchAll()
        {
           // store.Publish<Command>(new GetEventsByTypeAggregateStoreCommand() { AggregateType = typeof(T), callbackevents = new CommandAction<IDictionary<Guid, IEnumerable<Event>>>(PreFetch) });

        }

        //private void PreFetch(IDictionary<Guid, IEnumerable<Event>> obj)
        //{
        //    foreach (var events in obj.Values)
        //    {
        //        var entity = CreateAndLoadEvents(events);
        //        AddCache(entity);
        //    }
        //}


        // these saves are updates !
        public void Save(T aggregate)
        {
            Save(aggregate, null, null, -1);
        }





        public void Save(T aggregate, Action<T> success, Action<Exception, T> failure)
        {
            Save(aggregate, success, failure, -1);
        }

        public void Save(T aggregate, Action<T> success, Action<Exception, T> failure, int version)
        {
            currentId =  (currentId + 1);
            aggregate.SetId(currentId);
         //   EnsureCache(aggregate.Id);



            // bump ID  grow cache if needed
            // generate GUID 
            store.Publish(new SaveEventsStoreCommand(aggregate.GetUncommittedChanges(), version,new StoreCommandAction<int>(x => aggregate.MarkChangesAsCommitted()),new StoreCommandAction<Exception>(x => failure.Invoke(x, aggregate))  ));

            AddCache(aggregate); //FIXME only add on success
        }


        public void Dispose()
        {
      
        }


        public void MultiSave(IList<T> aggregate)
        {
            throw new NotImplementedException();
        }

        public T New()
        {
            throw new NotImplementedException();
        }
    }
}

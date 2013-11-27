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
using System.Diagnostics;

namespace L6.Infrastructure.EventSourcing.Disruptor
{

    //


    /// <summary>
    /// 
    /// Some nastieness to force external domain callbacks into the disruptor.. so the correct thread processes it 
    /// 
    /// IMHO Not bad and isolated to this class the rest of business Domain and Event store are unaware 
    /// 
    ///  talks to event store behind a Disruptor
    /// Default will eventualy store all values everything used in memory repsitory 
    /// Use for small to medium data for which there are few inserts and no deletes.
    /// 
    ///NOT threadsafe
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DisruptorRepository<T> : IDomainRepository<T>
        where T : EventSourcedAR<T>, new()
    {

        IEventPublisher eventPublisher;
        ICommandPublisher store;
        ICommandPublisher currentDomain;
        StoreCommandAction<Command> publisher;
        T[] cache;

        int highestIdentity; // static is meaningless


        /// <summary>
        /// count is very important to get right as new identities are issued based on this.
        /// 
        /// we could load all object and have the count but this way we only show objects used and more synergy 
        /// with a expiry cache implementation
        /// </summary>
        /// <param name="store"></param>
        /// <param name="Count"></param>
        public DisruptorRepository(ICommandPublisher store, ICommandPublisher currentDomain , IEventPublisher eventPublisher)
        {
            this.store = store;
            this.currentDomain = currentDomain;
            this.publisher = new StoreCommandAction<Command>(currentDomain.Publish);
            this.eventPublisher = eventPublisher;

        }


        event EventHandler completed;
        public event EventHandler CompletedSetup { add { completed += value; } remove { completed -= value; } }
        protected void OnCompleted(Object sender, EventArgs args)
        {
            if (completed != null)
                completed(sender, args);
        }


        public void Init(Object data)
        {
            if (highestIdentity != 0)
            {
                OnCompleted(this, null);
                return;
            }

            store.Publish(new GetHighestIdForTypeStoreCommand()
            {
                type = typeof(T).ToString(),
           //     callbackevent = new StoreCommandAction<int>(x => currentDomain.Publish(new ActionCommand(() =>  LoadCount   ( (int) ConvertGuid.FromGuid (x) ))))
                        callbackevent = new StoreCommandAction<int>( x => LoadCount( x))
            });

        }



        public void Init(int count)
        {
            if (count > int.MaxValue)
                throw new ArgumentOutOfRangeException("count must be < 2^31");
            LoadCount(count);
        }

        // dont need to worry about thread...
        private void LoadCount(int newId)
        {
            //currentId = int.FromGuid(value);
            cache = new T[ (int) newId];
            highestIdentity = (int) newId;
            EnsureCache((int) newId);

            OnCompleted(this, null);
        }


        public void GetById(int id, Action<T> success, System.Action<Exception> failure)
        {
            if (id < 1 || id > cache.Length - 1)
                throw new ArgumentException("id must be positive and less than the current id "); 

            T cacheValue = LoadCache( id);
            if (cacheValue != null)
            {
               success.Invoke(cacheValue);
               return;
            }

            store.Publish(new GetEventsForAggregateStoreCommand()
            {
                AggregateId = id,
                callbackevents = new StoreCommandAction<IEnumerable<DomainEvent>>
                    (x => currentDomain.Publish(new ActionCommand(() => GetByIdCallback(x, success , id)))),  
                type = typeof(T).ToString(),
                failure = new StoreCommandAction<Exception>(x => currentDomain.Publish(new ActionCommand(() => failure(x))))
            });
        }

        void GetByIdCallback(IEnumerable<DomainEvent> events, Action<T> success , int id)
        {
            var obj = CreateAndLoadEvents(id , events);
            SetCache(obj);
            success.Invoke(obj);
        }

        private T CreateAndLoadEvents(int id , IEnumerable<DomainEvent> events)
        {
            var obj = new T();
            obj.SetId(id); 
            obj.LoadFromHistoricalEvents(events);
            return obj;
        }

        T LoadCache(int  id)
        {
            //     EnsureCache(id);
            T cacheValue = cache[(int) id];
            return cacheValue;
        }

        void EnsureCache(int id)
        {
            if (id > cache.Length)
            {

                var cap = id + Math.Max(16, cache.Count() / 6);  // will grow internal collection , in frequent so we dont need to grow too quick
                var array = new T[cap];
                cache.CopyTo(array, 0);
                cache = array;
            }

        }


        void SetCache(T obj)
        {
            //   EnsureCache(obj.Id); // FAULTY ! 
            cache[obj.AggregateRootId] = obj;
        }




        //private void PreFetch(IDictionary<int, IEnumerable<Event>> obj)
        //{
        //    foreach (var events in obj.Values)
        //    {
        //        var entity = CreateAndLoadEvents(events);
        //        SetCache(entity);
        //    }
        //}


        // these saves are updates !
        public void Save(T aggregate)
        {
            Save(aggregate, null, null, -1);
        }


        //public void Save(AggregateRoot aggregate, int expectedVersion)
        //{
        //    _storage.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), expectedVersion);
        //}

        //public T GetById(int id)
        //{
        //    var obj = new T();//lots of ways to do this
        //    var e = _storage.GetEventsForAggregate(id);
        //    obj.LoadsFromHistory(e);
        //    return obj;
        //}




        public void Save(T aggregate, Action<T> success, Action<Exception, T> failure)
        {
            Save(aggregate, success, failure, -1);
        }

        public void Save(T aggregate, Action<T> success, Action<Exception, T> failure, int version)
        {
            if (aggregate.AggregateRootId == 0)
                throw new ArgumentException();


         //   EnsureCache(aggregate.AggregateRootId);



            // bump ID  grow cache if needed
            // generate int 
          //  store.Publish(new SaveEventsStoreCommand()
          //  {
          ////      AggregateId = ConvertGuid.ToGuid (aggregate.AggregateRootId),
          //      events = aggregate.GetUncommittedChanges(),
          //      expectedVersion = version,
          //      //success = new StoreCommandAction<int>(x => aggregate.MarkChangesAsCommitted()),
          //      success = new StoreCommandAction<int>(x => currentDomain.Publish(new ActionCommand(() =>  HandleSaveSuccess(aggregate, success, version) ))),
          //      //   failure = new StoreCommandAction<Exception>(x => failure.Invoke(x, aggregate))
          //      failure = new StoreCommandAction<Exception>(x => currentDomain.Publish(new ActionCommand(() => failure(x, aggregate)))),
          //  //    type = typeof(T).ToString()

          //  });

            store.Publish(new SaveEventsStoreCommand(aggregate.GetUncommittedChanges(), version, new StoreCommandAction<int>(x => currentDomain.Publish(new ActionCommand(() => HandleSaveSuccess(aggregate, success, version))
          )), failure != null ? new StoreCommandAction<Exception>(x => failure.Invoke(x, aggregate)) : null));

            //store.Publish(new SaveEventsStoreCommand(aggregate.GetUncommittedChanges(), version, new StoreCommandAction<int>(x => aggregate.MarkChangesAsCommitted()), new StoreCommandAction<Exception>(x => failure.Invoke(x, aggregate))));
        
        }


        private void HandleSaveSuccess(T aggregate, Action<T> success, int version)
        {
            var events = aggregate.GetUncommittedChanges().ToList();
            aggregate.MarkChangesAsCommitted(); // clears collection

            foreach (var @event in events)
                eventPublisher.Publish(@event);


            SetCache(aggregate);
            success(aggregate);




        }

        public void Dispose()
        {
      
        }


        public void MultiSave(IList<T> aggregates)
        {

            store.Publish(new SaveEventsStoreCommand(aggregates.SelectMany(x => x.GetUncommittedChanges()), 0 , new StoreCommandAction<int>(x => currentDomain.Publish(new ActionCommand(() => HandleMultiSuccess(aggregates) )))   ));
        }

        private void HandleMultiSuccess(IList<T> aggregates)
        {

            foreach (var aggregate in aggregates)
            {
                var events = aggregate.GetUncommittedChanges().ToList();
                aggregate.MarkChangesAsCommitted(); // clears collection

                foreach (var @event in events)
                    eventPublisher.Publish(@event);

                var typedAgg = aggregate as T;
                if ( typedAgg != null)
                    SetCache(typedAgg);
            }
        }

        public T New()
        {
            var aggregate = new T();
            highestIdentity = (int)(highestIdentity + 1);
            aggregate.SetId(highestIdentity);
            EnsureCache(highestIdentity);
            SetCache(aggregate);
            return aggregate;

        }
    }
}

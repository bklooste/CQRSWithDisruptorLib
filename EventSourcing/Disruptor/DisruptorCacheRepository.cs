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

    // talks to event store behind a Disruptor



    public class DisruptorCacheRepository<T> : IDomainRepository<T> 
        where T :  EventSourced , IAggregateRoot<T> , new()
    {
        int highestIdentity; // use Interloc.increment if you want thread safety

        IEventPublisher eventPublisher;
        int cacheSize = 1024; 
        ICommandPublisher store;
        ICommandPublisher currentDomain;
        Object data;
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

            public TimeOutHolder(long requestCountTimeStamp, T obj)
            {
                // TODO: Complete member initialization
                this.LastUsed = requestCountTimeStamp;
                this.Aggregate = obj;
            }
            public long LastUsed { get; set; }
            public T Aggregate { get; set; }
        }


        //static DisruptorCacheRepository()
        //{
        //    //poker

        //}

        /// count is very important to get right as new identities are issued based on this.
        /// 
        /// we could load all object and have the count but this way we only show objects used and more synergy 
        /// with a expiry cache implementation


       

        ///
        public DisruptorCacheRepository(ICommandPublisher store , ICommandPublisher currentDomain, IEventPublisher publishEvents, int cacheSize = 1024)
        {
            this.store = store;
            this.currentDomain = currentDomain;
            this.cacheSize = cacheSize;
            this.eventPublisher = publishEvents;
        }

        //FIXME is currentdomain needed !! Is it bad
        public void Init(Object data)
        {
            this.data = data; 

            if (highestIdentity != 0)
            {
                OnCompleted(this, null); 
                return;
            }

            var command = new GetHighestIdForTypeStoreCommand()
             {

                 type = typeof(T).ToString(),
                 //    callbackevent = new StoreCommandAction<Guid>(x => currentDomain.Publish(new ActionCommand(() => LoadCount((int)ConvertGuid.FromGuid(x)))))
                 callbackevent = new StoreCommandAction<int>(x => LoadCount(x))
             };

            if (command == null)
                throw new Exception("test"); 

             store.Publish(command);
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
            Debug.WriteLine(" Load count" + typeof(T).Name + " : " + newId);  
            if (highestIdentity == 0)
                highestIdentity = (int)newId;
            else
                Debugger.Break();
            OnCompleted(this, null);
        }


        public void GetById(int id, Action<T> success, System.Action<Exception> failure)
        {
            if (id < 1 )
                throw new ArgumentException("id must be positive and less than the current id ");

            if (id > highestIdentity)
                Debugger.Break();


            T cacheValue = LoadCache(id);
            if (cacheValue != null)
            {
                success.Invoke(cacheValue);
                return;
            }

            store.Publish(new GetEventsForAggregateStoreCommand()
            
            {
                
                AggregateId = id,
                callbackevents = new StoreCommandAction<IEnumerable<DomainEvent>>
                             (x => currentDomain.Publish(new ActionCommand(() => GetByIdCallback(x, success, id)))),
                type = typeof(T).ToString(),
                failure = new StoreCommandAction<Exception>(x => currentDomain.Publish(new ActionCommand(() => failure(x))))
            });
        }



        void GetByIdCallback(IEnumerable<DomainEvent> events, Action<T> success , int id )
        {
            var obj = CreateAndLoadEvents(id, events);
            
                EnsureInCache(obj);
            success.Invoke(obj);
        }

        private T CreateAndLoadEvents(int id, IEnumerable<DomainEvent> events)
        {
            var obj = new T();
            obj.SetId(id); 
            obj.LoadFromHistoricalEvents(events);


            if (obj is IRequiresData)
                ((IRequiresData)obj).SetData(this.data); // A bit hackish doing it in the repository , should be more generic..



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

      


        void EnsureInCache(T obj)
        {
            //   EnsureCache(obj.Id); // FAULTY ! 
            if (cache.ContainsKey(obj.AggregateRootId) == false)
            {
                cache.Add(obj.AggregateRootId, new TimeOutHolder(requestCountTimeStamp, obj));


                if (cache.Count > cacheSize +1 )  // evict old 
                {
                    try
                    {
                        var valsToremove = cache.Values.OrderBy(x => x.LastUsed).Take(Math.Max(cache.Count - cacheSize, cacheSize / 10));

                        foreach (var item in valsToremove)
                            cache.Remove(item.Aggregate.AggregateRootId);
                    }
                    catch (Exception)
                    {
                        Debugger.Break(); 
                    }
                }
            }
        }


        // these saves are updates !
        public void Save(T aggregate)
        {
            Save(aggregate, null, null, -1);
        }





        public void Save(T aggregate, Action<T> success, Action<Exception, T> failure)
        {
            Save(aggregate, success, failure, -1);
        }


        /// <summary>
        /// Ids must be set before hand  , use New() !
        /// </summary>
        /// <param name="aggregate"></param>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        /// <param name="version"></param>
        public void Save(T aggregate, Action<T> success, Action<Exception, T> failure, int version)
        {
            if (aggregate.AggregateRootId == 0)
                throw new ArgumentException();


            if (aggregate.GetUncommittedChanges().Count() == 0)
            {
                Debug.WriteLine("no changes found");
                return;
            }

            //
            var events  = aggregate.GetUncommittedChanges();
            int seq = aggregate.Sequence;
            foreach (var @event in events)
            {
                if (@event.Sequence != 0)
                    continue;  // FIXME concurrent save to disk 

                seq++;
                @event.Sequence = seq;
            }

            // for when using manual ids
            if (aggregate.AggregateRootId > highestIdentity)
            {
                highestIdentity = aggregate.AggregateRootId;
                Debug.WriteLine("Indentity set to " + highestIdentity + "for " + aggregate.GetType().Name ); 
            }

            store.Publish(new SaveEventsStoreCommand(events, version, new StoreCommandAction<int>(x => currentDomain.Publish(new ActionCommand(() => HandleSaveSuccess(aggregate, success, version))
                )), failure != null ? new StoreCommandAction<Exception>(x => failure.Invoke(x, aggregate)) : null));
            //store.Publish(new SaveEventsStoreCommand(aggregate.GetUncommittedChanges(), version, 
            //    success != null ?  new StoreCommandAction<Guid>(x => aggregate.MarkChangesAsCommitted()) : null, 
            //    failure != null ? new StoreCommandAction<Exception>(x => failure.Invoke(x, aggregate)) : null 
            //    ));

           

          
        }

        //FIXME tests  on identity map remove 
        //FIXME is it ok for errors to come on different thread ?
        private void SaveFailure(T aggregate, Action<Exception, T> failure, Exception x)
        {
            if ( cache.ContainsKey(aggregate.AggregateRootId))
                cache.Remove(aggregate.AggregateRootId);
            if (failure != null)
                currentDomain.Publish(new ActionCommand(() => failure(x, aggregate)));
            else
                Debug.WriteLine("save failure no handler " + x.ToString()); 
        }




        private void HandleSaveSuccess(T aggregate, Action<T> success , int version)
        {
            var events = aggregate.GetUncommittedChanges().ToList(); 
            aggregate.MarkChangesAsCommitted(events.Max (x=>x.Sequence)); //  fixme.. should track sequence list of success and just clear. 

            //set aggregaye

            foreach (var @event in events)
                eventPublisher.Publish(@event);

         
            EnsureInCache(aggregate); //FIXME only add on success
            if ( success != null)
               success(aggregate);

            //List<EventDescriptor> eventDescriptors;
            //if (!_current.TryGetValue(aggregateId, out eventDescriptors))
            //{
            //    eventDescriptors = new List<EventDescriptor>();
            //    _current.Add(aggregateId, eventDescriptors);
            //}
            //else if (eventDescriptors[eventDescriptors.Count - 1].Version != expectedVersion && expectedVersion != -1)
            //{
            //    throw new ConcurrencyException();
            //}


            //var i = expectedVersion;
            //foreach (var @event in events)
            //{
            //    i++;
            //    @event.Version = i;
            //    eventDescriptors.Add(new EventDescriptor(aggregateId, @event, i));
            //    _publisher.Publish(@event);
            //}
           // ihio
        }


        public void Dispose()
        {
      
        }


        //FIXME multi save will require store changes...this would allow multiple 
        // aggregates saved in 1 command = new query 
        public void MultiSave(IList<T> aggregates)
        {

            if (aggregates.SelectMany(x => x.GetUncommittedChanges()).Count() == 0)
            {
                Debug.WriteLine("no changes found in multi save");
                return;
            }

            store.Publish(new SaveEventsStoreCommand(aggregates.SelectMany(x => x.GetUncommittedChanges()), 0, new StoreCommandAction<int>(x => currentDomain.Publish(new ActionCommand(() => HandleMultiSuccess(aggregates))))));

            //store.Publish(new SaveEventsStoreCommand()
            //{
            //    //   AggregateId = ConvertGuid.ToGuid(aggregate.AggregateRootId),
            //    events = aggregates.SelectMany(x => x.GetUncommittedChanges()),
            //    success = new StoreCommandAction<Guid>(x => currentDomain.Publish(new ActionCommand(() => HandleMultiSuccess(aggregates)))),

            //});
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
                if (typedAgg != null)
                    EnsureInCache(typedAgg);
            }
        }

        public T New()
        {
            var aggregate = new T();
            highestIdentity = highestIdentity + 1;
            aggregate.SetId(highestIdentity);

           
                EnsureInCache(aggregate);

                if (aggregate is IRequiresData)
                    ((IRequiresData)aggregate).SetData(this.data); // A bit hackish doing it in the repository , should be more generic..

            return aggregate;

        }
    }
}

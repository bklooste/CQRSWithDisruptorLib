using L6.Infrastructure.Events;
using L6.Infrastructure.Events.EventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L6.Infrastructure.Util;

namespace L6.Infrastructure.Domain
{
    // faulty !
    public class StatelessRepository<T> : IRepository<T> where T:AggregateRoot , new()
    {
        IEventStoreExtended store;
        IList<T> cache = new List<T>(); 
        
        static int maxValue;


        public StatelessRepository(IEventStoreExtended store)
        {
            this.store = store;
        }
        public void GetById(Util.Id<int> id, System.Action<T> success, System.Action<Exception , Id<int>> failure)
        {
            try
            {
                var obj = new T();
                var e = store.GetEventsForObject(id.ToGuid());
                obj.LoadsFromHistory(e);
                success.Invoke(obj);
            }
            catch (Exception ex)
            {
                failure.Invoke(ex, id); 
            }
        }


        //meaningless as Repository has no state
        public bool TryGet(Util.Id<int> id, out T val)
        {
            val = null;
            return false;
        }

        //meaningless as Repository has no state
        public void Prefetch()
        {
          return;  // ignore
        }

        public void Save(T aggregate)
        {
            store.SaveEvents(aggregate.Id.ToGuid(), aggregate.GetUncommittedChanges(), -1 );
        }

        public void Save(T aggregate, System.Action<T> success, System.Action<Exception> failure)
        {
            Save(aggregate, success, failure, -1);
        }

        public void Save(T aggregate, System.Action<T> success, System.Action<Exception , T> failure, int version)
        {
            store.SaveEvents(aggregate.Id.ToGuid(),
                aggregate.GetUncommittedChanges(),
                version);

            success.Invoke(aggregate); 

        }

        public void Dispose()
        {
          
        }

        //public void Save(AggregateRoot aggregate, int expectedVersion)
        //{
        //    _storage.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), expectedVersion);
        //}

        //public T GetById(Guid id)
        //{
        //    var obj = new T();//lots of ways to do this
        //    var e = _storage.GetEventsForAggregate(id);
        //    obj.LoadsFromHistory(e);
        //    return obj;
        //}




        public void PrefetchAll()
        {
            throw new NotImplementedException();
        }

        public void Save(T aggregate, Action<T> success, Action<Exception> failure)
        {
            throw new NotImplementedException();
        }

       

        void IRepository<T>.GetById(Util.Id<int> id, Action<T> success, Action<Exception> failure)
        {
            throw new NotImplementedException();
        }

        bool IRepository<T>.TryGet(Util.Id<int> id, out T val)
        {
            throw new NotImplementedException();
        }

        void IRepository<T>.PrefetchAll()
        {
            throw new NotImplementedException();
        }

        void IRepository<T>.Save(T aggregate)
        {
            throw new NotImplementedException();
        }

        void IRepository<T>.Save(T aggregate, Action<T> success, Action<Exception> failure)
        {
            throw new NotImplementedException();
        }

        void IRepository<T>.Save(T aggregate, Action<T> success, Action<Exception> failure, int version)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

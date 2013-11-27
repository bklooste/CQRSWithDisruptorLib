using L6.Infrastructure.Events;
using L6.Infrastructure.Events.EventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.Domain
{
    //single threaded
    public class IdentityMapRepository<T> : IRepository<T> where T : AggregateRoot
    {
        IEventStore store;
        static IList<T> cache = new List<T>(); // clear cache when ! 
        int maxValue;


        public IdentityMapRepository(IEventStore store)
        {
            this.store = store;
        }
        public void Find(Util.Id<int> id, System.Action<T, Commands.Command> success, System.Action<T> failure)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGet(Util.Id<int> id, out T val)
        {
            throw new System.NotImplementedException();
        }

        public void PrefetchAll()
        {
            throw new System.NotImplementedException();
        }

        public void Save(T aggregate)
        {
            throw new System.NotImplementedException();
        }

        public void Save(T aggregate, System.Action<T> success, System.Action<T> failure)
        {
            throw new System.NotImplementedException();
        }

        public void Save(T aggregate, System.Action<T> success, System.Action<T> failure, int version)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void GetById(Util.Id<int> id, Action<T> success, Action<Exception> failure)
        {
            throw new NotImplementedException();
        }

        public void Save(T aggregate, Action<T> success, Action<Exception> failure)
        {
            throw new NotImplementedException();
        }

        public void Save(T aggregate, Action<T> success, Action<Exception> failure, int version)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
namespace L6.Infrastructure.Domain
{
    public interface IAggregateRootCollection<T> : IFixedAggregateRootCollection<T> 
     where T : AggregateRoot, new()
    {
        void Add(T item);
    //    System.Collections.Generic.IEnumerator<T> GetEnumerator();
    }



    public interface IFixedAggregateRootCollection<T> : IEnumerable<T> //, IDo
   where T : AggregateRoot, new()
    {
        T this[int index] { get;  }
     
        //    System.Collections.Generic.IEnumerator<T> GetEnumerator();
    }
}

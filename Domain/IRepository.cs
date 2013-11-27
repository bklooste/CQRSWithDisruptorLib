using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L6.Infrastructure.Commands;
using L6.Infrastructure.Domain;
using L6.Infrastructure.Util;

namespace L6.Infrastructure.Domain
{

    /// <summary>
    ///  Looks like repositories call beyond the Disruptor boundary ??? ( look at the API ) .. 
    ///  
    /// Note repository is responsible of converting Domain Ids to Event store Guids and generating Ids
    /// </summary>
    public interface IDomainRepository
    {
        void Init(Object data = null); 
        event EventHandler CompletedSetup ;
    };


    // some repositoryiex may need a Func factory
    public interface IDomainRepository<T> : IDomainRepository, IDisposable //IPersistContext<T>  ,
         where T : IAggregateRoot<T>
    {
        //most reps dont need but we do as we need the Id in the repository

        void GetById(int id, Action<T> success, Action<Exception> failure);

        //bool TryGet(uint id, out T val);
        //void PrefetchAll();    // would only be used with a store that temporarily increased its capacity  

        //BUG cashing does not update all the aggregate types...
        void MultiSave(IList<T> aggregate); // this is dodgy , belongs to the type but then persistant mechaism is locked in ..

        T New(); // creates a new object with a valid ID - not persistsed ! .   // handkers can use

        void Save(T aggregate);
        void Save(T aggregate, Action<T> success, Action<Exception, T> failure); // tie exception to entity 
        void Save(T aggregate, Action<T> success, Action<Exception , T> failure, int version);
    }
}

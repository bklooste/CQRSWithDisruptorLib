
//using System;
//using System.Collections.Generic;
//using L6.Infrastructure.Events;
//using System.ComponentModel.DataAnnotations;
//using Cont = System.Diagnostics.Contracts;
//using L6.Infrastructure.Commands;

//namespace L6.Infrastructure.Domain
//{

//    // public class InventoryCommandHandlers
//    //{
//    //    private readonly IRepository<InventoryItem> _repository;
//    //    public InventoryCommandHandlers(IRepository<InventoryItem> repository)
//    //    {
//    //        _repository = repository;
//    //    }



//    /// <summary>
//    /// aggregate root S , command Handler T
//    /// </summary>
//    /// <typeparam name="S"></typeparam>
//    /// <typeparam name="T"></typeparam>
//    public abstract class TwoPhaseCommandHandler<S, T> : ICommandHandler<T>
//        where T : PersistantCommand<S>
//        where S : AggregateRoot
//    {
//        protected IRepository<S> repository;


//        // type 1 
//        // create / fetch domain object 
//        // apply logic
//        // persist domain object

//        // type2
//        // fetch domain object via callback   ( could be cached in EF) 
//        // we can do a tryGet on the cache  and only callback if it fails..
//        // apply logic 
//        // persist


//        public TwoPhaseCommandHandler(IRepository<S> context)
//        {
//            this.repository = context;
//        }





//    //    public void Handle(T args)
//    //    {
//    //        // Get Entity FOr S
//    //        S val = default(S);
//    //        if (repository.TryGet(args.Id, out val))
//    //            Phase2Handle(val);
//    //        else

//    //            //finish and continue on a callback
//    //            repository.Find(args.Id, new Action<S>(Phase2Handle), null);
//    //    }

//    //    public abstract void Phase2Handle(S val);

//    //    public void Handle(Command args)
//    //    {
//    //        Handle(args as T);
//    //    }

//    //}
//}

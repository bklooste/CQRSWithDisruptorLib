//using System;
//using System.Collections.Generic;
//using Autofac;


//namespace L6.DomainEventInfrastructure
//{

//    public interface IDomainEvent { }

//    public interface DomainEventHandles<T> where T : IDomainEvent
//     {
//         void Handle(T args); 
//     } 


//  //  public class CustomerBecamePreferred : IDomainEvent 
//  // 2:  {
//  // 3:      public Customer Customer { get; set; }
//  // 4:  }
//  //  public class Customer
//  // 2:  {
//  // 3:      public void DoSomething()
//  // 4:      {
//  // 5:          DomainEvents.Raise(new CustomerBecamePreferred() { Customer = this });
//  // 6:      }



//  //  // call event
//  //  public void Handle(DoSomethingMessage msg)
//  // 2:  {
//  // 3:      using (ISession session = SessionFactory.OpenSession())
//  // 4:      using (ITransaction tx = session.BeginTransaction())
//  // 5:      {
//  // 6:          var c = session.Get<Customer>(msg.CustomerId);
//  // 7:          c.DoSomething();
//  // 8:   
//  // 9:          tx.Commit();
//  //10:      }
//  //11:  }

////    //test
////     public void DoSomethingShouldMakeCustomerPreferred()
////   2:  {
////   3:      var c = new Customer();
////   4:      Customer preferred = null;
////   5:   
////   6:      DomainEvents.Register<CustomerBecamePreferred>(
////   7:          p => preferred = p.Customer
////   8:              );
////   9:   
////  10:      c.DoSomething();
////  11:      Assert(preferred == c && c.IsPreferred);
////  12:  }

////Notice that the key word in the requirement – “when”.

////Any time you see that word in relation to your domain, consider modeling it as a domain event.

////So, here’s the handling code:

////   1:  public class CustomerBecamePreferredHandler : Handles<CustomerBecamePreferred>
////   2:  { 
////   3:     public void Handle(CustomerBecamePreferred args)
////   4:     {
////   5:        // send email to args.Customer
////   6:     }
////   7:  } 
////This code will run no matter which service layer object we came in through.

//    //public class DomainEvent<E> 
//    //{
//    //    [ThreadStatic] 
//    //    private static List<Action<E>> _actions; 

//    //    protected List<Action<E>> actions 
//    //    {
//    //        get { 
//    //            if (_actions == null) 
//    //                _actions = new List<Action<E>>(); 

//    //            return _actions; 
//    //        }
//    //    }

//    //    public IDisposable Register(Action<E> callback) 
//    //    {
//    //        actions.Add(callback);
//    //        return new DomainEventRegistrationRemover(delegate
//    //            {
//    //                actions.Remove(callback);
//    //            }
//    //        ); 
//    //    }

//    //    public void Raise(E args) 
//    //    {
//    //        foreach (Action<E> action in actions) 
//    //            action.Invoke(args);
//    //    }
//    //}


//    //public class SendEmailIfCustomerHasUnpaidDues : IEventHandler<CustomerHasUnpaidDuesEvent>
//    //{
//    //    public IEmailSender EmailSender { get; set; }
//    //    public void Handle(CustomerHasUnpaidDuesEvent @event)
//    //    {
//    //        EmailSender.SendEmail(@event.Subject.EmailAddress);
//    //    }
//    //}




//    // Use of container makes it static 
//    public static class GlobalDomainEvents
//    { 
//        [ThreadStatic] //so that each thread has its own callbacks
//        private static List<Delegate> actions;
     
//        public static IContainer Container { get; set; } //as before
     
//        //Registers a callback for the given domain event
//        public static void Register<T>(Action<T> callback) where T : IDomainEvent
//        {
//           if (actions == null)
//              actions = new List<Delegate>();
     
//           actions.Add(callback);
//       }
     
//       //Clears callbacks passed to Register on the current thread
//       public static void ClearCallbacks ()
//       {
//           actions = null;
//       }
     
//       //Raises the given domain event
//       public static void Raise<T>(T args) where T : IDomainEvent
//       {
//          if (Container != null)
//             foreach(var handler in Container.Resolve<IEnumerable<DomainEventHandles<T>>>())
//                handler.Handle(args);
    
//          if (actions != null)
//              foreach (var action in actions)
//                  if (action is Action<T>)
//                      ((Action<T>)action)(args);
//       }
//    } 
////Notice that while this class *can* use a container, the container isn’t needed for unit tests which use the Register method.

////When used server side, please make sure that you add a call to ClearCallbacks in your infrastructure’s end of message processing section. In nServiceBus this is done with a message module like the one below:

//    public class DomainEvents
//    {
//      //  [ThreadStatic] //so that each thread has its own callbacks
//        private static List<Delegate> actions;

      
//        //Registers a callback for the given domain event
//        public void Register<T>(Action<T> callback) where T : IDomainEvent
//        {
//            if (actions == null)
//                actions = new List<Delegate>();

//            actions.Add(callback);
//        }

//        //Clears callbacks passed to Register on the current thread
//        public void ClearCallbacks()
//        {
//            actions = null;
//        }

//        //Raises the given domain event
//        public void Raise<T>(T args) where T : IDomainEvent
//        {
//            //if (Container != null)
//            //    foreach (var handler in Container.Resolve<IEnumerable<DomainEventHandles<T>>>())
//            //        handler.Handle(args);

//            if (actions != null)
//                foreach (var action in actions)
//                    if (action is Action<T>)
//                        ((Action<T>)action)(args);
//        }
//    } 





//   //public class DomainEventsCleaner : IMessageModule
//   //{ 
//   //      public void HandleBeginMessage() { }
      
//   //      public void HandleEndMessage()
//   //      {
//   //          DomainEvents.ClearCallbacks();
//   //      }


//   //      public void HandleError()
//   //      {
//   //          throw new NotImplementedException();
//   //      }
//   //}


//   /// <summary>
//   /// Implementers will be called before and after all message handlers.
//   /// </summary>
//   public interface IMessageModule
//   {
//       /// <summary>
//       /// This method is called before any message handlers are called.
//       /// </summary>
//       void HandleBeginMessage();

//       /// <summary>
//       /// This method is called after all message handlers have been called.
//       /// </summary>
//       void HandleEndMessage();

//       /// <summary>
//       /// This method is called if there was an error in processing the message,
//       /// and may be called after <see cref="HandleEndMessage"/>.
//       /// </summary>
//       void HandleError();
//   }

//}


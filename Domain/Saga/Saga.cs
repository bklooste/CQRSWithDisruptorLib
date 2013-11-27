using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using L6.Data.Infrastructure;
using L6.Infrastructure.Commands;
using L6.Infrastructure.Events;

namespace L6.Infrastructure.Domain.Sagas
{
    //FIXME merge better with eventsource AR
    public abstract class  Saga<U,T> : SagaBase,  IAggregateRoot<T>,
        IStartedByEvent<U>    // create when we get this event

          where U : DomainEvent
    {
        public abstract void Start(U val);
        protected int id;
        protected abstract override void Dispatch(DomainEvent @event);

        public int AggregateRootId
        {
            get { return id; }
        }

        public void SetId(int id)
        {
            this.id = id;
        }

        protected new void Update(DomainEvent @event)
        {
            @event.SourceAggregateRootId = id;

            base.Update(@event);
        }
    }


    // we could force process managers to create sagas via a class that would simplify the wiring up ..
    // try to keep process managers command only and sagas domain events
    public interface  IProcessManager :   ICommandHandler // experimental !

    {
        ICommandPublisher Publisher { set; } 
     
    }

    public interface IEventProcessor : IHandles// experimental !
    {
        ICommandPublisher Publisher { set; }

    }

    


    /// <summary>
    /// Saga receives messages , not commands !
    /// This has some advantages in distributed systems and the cost of a single handler to create the event is low.. 
    /// 
    /// we may have process managers which receive commands..
    /// 
    /// 
    /// This class is used to define sagas containing data and handling a message.
    /// To handle more message types, implement <see cref="IMessageHandler{T}"/>
    /// for the relevant types.
    /// To signify that the receipt of a message should start this saga,
    /// implement <see cref="ISagaStartedBy{T}"/> for the relevant message type.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="ISagaEntity"/>.</typeparam>
    public abstract class
        SagaBase : EventSourced, 
        IDisposable,   // sagas own resources which may need to be cleaned up.
        IHandles    //when created hooks event bus ...
    {

        IDisposable eventBusSubscriptionToken;

        ICommandPublisher commandBus;
        CancellationToken cancelToken;
   //     Task timeoutTask; //TODO only have external control now

        public SagaBase()
        {
        }

        public async void SetTimeOut(TimeSpan time)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            this.cancelToken = cts.Token;
            await Task.Delay(time.Milliseconds, cancelToken);
            TimeOut();
        }


        public ICommandPublisher CommandBus
        {
            get { return commandBus; }
            set { commandBus = value; }
        }


        public IDisposable EventBusSubscriptionToken
        {
            get { return eventBusSubscriptionToken; }
            set { eventBusSubscriptionToken = value; }
        }

        public bool Completed { get; private set; }


        /// <summary>
        /// Marks the saga as complete.
        /// This may result in the sagas state being deleted by the persister.
        /// </summary>
        protected virtual void MarkAsComplete()
        {
            Completed = true;
        }


        /// <summary>
        /// Message handler for Timeout Message 
        /// </summary>
        /// <param name="message">Timeout Message</param>
        /// 

        // we do a timeout  via a command direct to the command processor  
        //( this is on a different thread - if we added it to the event bus we would need to set the thread.  ) . 
        protected virtual void TimeOut()
        {
            //Timeout(message.State);
        }



        public void Dispose()
        {
            eventBusSubscriptionToken.Dispose();
        }

        public abstract void Start(DomainEvent val);

    }

}

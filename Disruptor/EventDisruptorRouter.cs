using System;
using System.Collections.Generic;
using L6.Infrastructure.Events;
using L6.Infrastructure.Domain;

using System.Threading.Tasks;


namespace L6.Infrastructure.Disruptor
{

    // base domain class holds the dipatching note domains can nest domains. 
    public class EventDisruptorRouter : IEventPublisher
    {
        IEventPublisher dispatcher;
        IList<IEventPublisher> domains;

        /// <summary>
        /// domains must be in a list where game number is the index...
        /// </summary>
        /// <param name="domains"></param>
        public EventDisruptorRouter(IList<IEventPublisher> domains)
        {
            //EventHandlers = new List<IEventHandler>();
            //EventBus = new EventRouter() as IEventBus;

            var handle1 = new DisruptorActionCommandEventHandler<EventHolder>(new Action<EventHolder>(ProcessEvent));

            var eventDispatch = new EventDispatcher(handle1);
            dispatcher = eventDispatch;
            Task.Factory.StartNew(() => eventDispatch.Start(), TaskCreationOptions.LongRunning);
            this.domains = domains; 
        }



        public void ProcessEvent(EventHolder Event) 
        {
            //EventRouter 

            domains[Event.Value.domainId].Publish(Event.Value); 
        }

        public IEventPublisher Publisher
        {
            get { return dispatcher; }
        }

        public void Publish<T>(T args) where T : DomainEvent
        {
            dispatcher.Publish<T>(args); 
        }


        public void Publish(DomainEvent @event)
        {
            dispatcher.Publish(@event); 
        }
    }
}

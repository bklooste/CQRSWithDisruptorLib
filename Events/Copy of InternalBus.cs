using System;
using System.Linq;
using System.Collections.Generic;
using Autofac;


namespace L6.Infrastructure.Events
{



    // not thread safe
    public class InternalEventBus : IEventBus
    {
        private Dictionary<Type, List<Delegate>> actions; // consider linked list and move to front on requests

        public InternalEventBus()
        {
            actions = new Dictionary<Type, List<Delegate>>();

        }




        // throws if type if not registered
        public void ClearCallbacks<T>()
        {
            var key = typeof(T);
            var list = actions[key];
            list.Clear();
        }

        public void ClearAllCallbacks()
        {
            actions = new Dictionary<Type, List<Delegate>>();
        }

        //Registers a callback for the given domain event
        public IDisposable Subscribe<T>(Action<T> callback) where T : Event
        {
            return Subscribe(typeof(T), (Delegate)callback);
        }

        public IDisposable Subscribe(Type t, Delegate delegate1)
        {
            var key = t;

            //   var key = callback.Method.GetHashCode();
            if (!actions.ContainsKey(key))
                actions.Add(key, new List<Delegate>());

            actions[key].Add(delegate1);

            return new DomainEventRegistrationRemover(delegate
            {
                actions[key].Remove(delegate1);
            });

        }

        //public void RegisterHandler<T>(Action<T> handler) where T : Message
        //{
        //    List<Action<Message>> handlers;
        //    if (!_routes.TryGetValue(typeof(T), out handlers))
        //    {
        //        handlers = new List<Action<Message>>();
        //        _routes.Add(typeof(T), handlers);
        //    }
        //    handlers.Add(DelegateAdjuster.CastArgument<Message, T>(x => handler(x)));
        //}



        // a bit slow dont use too often
        public void SubscribeHandler(IHandles eventHandler)
        {
            var genericHandler = typeof(IHandles<>);
            var supportedEventTypes = eventHandler.GetType()
                .GetInterfaces()
                .Where(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == genericHandler)
                .Select(iface => iface.GetGenericArguments()[0])
                .ToList();

            // Register this handler for each of the handled types.
            foreach (var eventType in supportedEventTypes)
            {
                Subscribe(eventType.GetType(), Delegate.CreateDelegate(eventType, eventHandler, "Handle")); 
            }
        }




        //Raises the given domain event
        public void Publish<T>(T args) where T : Event
        {
            PublishInternal(typeof(T) , args   );
        }

        private void PublishInternal(Type key , Event args) 
        {
            List<Delegate> value;
            actions.TryGetValue(key, out value);
            if (value != null)
            {
                for (int i = 0; i < value.Count; i++)
                    ((Action<Event>)value[i])(args);
            }


            // capture "Event" type logging all events etc 

            if (this.actions.TryGetValue(typeof(Event), out value))
                for (int i = 0; i < value.Count; i++)
                    ((Action<Event>)value[i])(args);
        }


        public void Publish(Event @event)
        {
            PublishInternal(@event.GetType(), @event); 
        }
    }





}


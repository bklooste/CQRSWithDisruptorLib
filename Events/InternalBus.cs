using System;
using System.Linq;
using System.Collections.Generic;
using Autofac;
using System.Diagnostics;
using System.Linq.Expressions;
using L6.Infrastructure.Util;


namespace L6.Infrastructure.Events
{



    // not thread safe

    /// <summary>
    /// Note that subscriptions are actual and not for sub types..
    /// 
    /// Subscription to Event will get all events
    /// </summary>
    public class MemoryEventBus : IEventBus
    {
        private IDictionary<Type, ICollection<IHandles>> actions; 

        public MemoryEventBus()
        {
            actions = new Dictionary<Type, ICollection<IHandles>>();

        }

        // throws if type if not registered
        public void ClearCallbacks<T>()
        {
            var key = typeof(T);
            if (!actions.ContainsKey(key))
                return;

            var list = actions[key];
            list.Clear();
        }

        public void ClearAllCallbacks()
        {
            actions = new Dictionary<Type, ICollection<IHandles>>();
        }

        //Registers a callback for the given domain event
        public IDisposable Subscribe<T>(Action<T> callback) where T : DomainEvent
        {
            return Subscribe(typeof(T), new ActionHandler<T>(callback));
        }

        //public IDisposable Subscribe(Type t, Expression delegate1)
        //{
        //    var key = t;

        //    //   var key = callback.Method.GetHashCode();
      
        //    Action<EventHandler> handler = DelegateAdjuster.CastArgument

        //    var action = delegate1 as Action<DomainEvent>;
        //    if (action == null)
        //        throw new InvalidCastException("Could not cast delegate to action");

        //    return Subscribe(key, new ActionHandler(action));


        //}

        public IDisposable Subscribe(Type key, IHandles handler)
        {
            Ensure.NotNull<IHandles>(handler, "handler");

            if (!actions.ContainsKey(key))
                actions.Add(key, new LinkedList<IHandles>());
            actions[key].Add(handler);


            return new DomainEventRegistrationRemover(delegate
            {
                actions[key].Remove(handler);
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
        public IDisposable SubscribeHandler(IHandles eventHandler)
        {
            var genericHandler = typeof(IHandles<>);
            var supportedEventTypes = eventHandler.GetType()
                .GetInterfaces()
                .Where(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == genericHandler)
                .Select(iface => iface.GetGenericArguments()[0])
                .ToList();

            // Register this handler for each of the handled types.
           // var unsubscribe = new List<IDisposable>();
            foreach (var eventType in supportedEventTypes)
            {
                Subscribe(eventType, eventHandler);
            }

              return new DomainEventRegistrationRemover(delegate
            {
                foreach ( var handlerList in actions.Values)
                {
                    foreach ( var val in handlerList)
                    {
                        if ( val == eventHandler )
                            handlerList.Remove(val);
                    }
           
                }
            });
        }






        //Raises the given domain event
        public void Publish<T>(T args) where T : DomainEvent
        {
            PublishWithType(typeof(T) , args   );
        }

        public void Publish(DomainEvent @event)
        {
            PublishWithType(@event.GetType(), @event);
        }


        public void PublishWithType(Type key , DomainEvent args) 
        {
            ICollection<IHandles> value;
            actions.TryGetValue(key, out value);
            if (value != null)
            {
                foreach(var handles in value)
                {
                    //Trace.WriteLine("-- Event Handled by " + handles.GetType().FullName);
                    ((dynamic)handles).Handle((dynamic)args);
                }
            }


            // capture "Event" type logging all events etc 

            if (this.actions.TryGetValue(typeof(DomainEvent), out value))
               foreach(var handles in value)
               {
                   //Trace.WriteLine("-- Event Handled by " + handles.GetType().FullName);
                   ((dynamic)handles).Handle((dynamic)args);
               }
        }



        //private void PublishWithType(Command args, Type commandType)
        //{
        //    ICommandHandler handler = null;

        //    if (this.handlers.TryGetValue(commandType, out handler))
        //    {
        //        Trace.WriteLine("-- Handled by " + handler.GetType().FullName);
        //        ((dynamic)handler).Handle((dynamic)args);
        //    }

        //    // There can be a generic logging/tracing/auditing handlers
        //    // if an issue precan a seperate collection for Icommand
        //    if (this.handlers.TryGetValue(typeof(Command), out handler))
        //    {
        //        Trace.WriteLine("-- Handled by " + handler.GetType().FullName);
        //        ((dynamic)handler).Handle((dynamic)args);
        //    }
        //}

    }





}


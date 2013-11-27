using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
//using Autofac;


namespace L6.Infrastructure.Commands
{


    //  public delegate void DomainDelegate(Command s);

    /// <summary>
    /// This is an instanced class , access via 
    /// 1. Immediate Events -domainRoot ( which is threadstatic and set in the disruptor handle) 
    /// 2. Callback Events - these should create a command ( See below) 
    /// 
    /// Also note command access is via 
    /// DomainDispatcher ( internal )  which is also on the DomainRoot
    /// MultipleDomainCOmmandDispatcher ( external) 
    /// </summary>
    public class CommandRouter : ICommandBus
    {
        //   private IDictionary<int, Delegate> actions;
        private Dictionary<Type, List<ICommandHandler>> handlers = new Dictionary<Type, List<ICommandHandler>>();

        public CommandRouter()
        {

        }


        public void Publish<T>(T args) where T : Command
        {
            PublishWithType(args, typeof(T)); 
        }

        //Raises the given domain event
        public void Publish(Command args) 
        {


            var commandType = args.GetType();
            PublishWithType(args, commandType);

        }

        private void PublishWithType(Command args, Type commandType)
        {
            List<ICommandHandler> handlerList = null;

            if (this.handlers.TryGetValue(args.GetType(), out handlerList))
            {
                foreach ( var handler in handlerList)
                //Trace.WriteLine("-- Handled by " + handler.GetType().FullName);
                ((dynamic)handler).Handle((dynamic)args);
            }

            // There can be a generic logging/tracing/auditing handlers
            // if an issue precan a seperate collection for Icommand
            if (this.handlers.TryGetValue(typeof(Command), out handlerList))
            {
                foreach (var handler in handlerList)

                //Trace.WriteLine("-- Handled by " + handler.GetType().FullName);
                ((dynamic)handler).Handle((dynamic)args);
            }
        }





        // a bit slow dont use too often
        public void Register(ICommandHandler commandHandler)
        {
            var genericHandler = typeof(ICommandHandler<>);
            var supportedCommandTypes = commandHandler.GetType()
                .GetInterfaces()
                .Where(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == genericHandler)
                .Select(iface => iface.GetGenericArguments()[0])
                .ToList();

            if (handlers.Keys.Any(registeredType => supportedCommandTypes.Contains(registeredType)))
                throw new ArgumentException("The command handled by the received handler already has a registered handler.");

            // Register this handler for each of the handled types.
            foreach (var commandType in supportedCommandTypes)
            {
                var list = this.handlers[commandType];
                if (list == null)
                    list = new List<ICommandHandler>();
                list.Add(commandHandler);
            }
        }


        public void ClearCallback<T>() where T : Command
        {
            handlers.Remove(typeof(T));
        }
    }





}


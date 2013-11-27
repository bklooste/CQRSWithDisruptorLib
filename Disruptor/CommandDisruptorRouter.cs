using System;
using System.Collections.Generic;
using L6.Infrastructure.Commands;
using L6.Infrastructure.Domain;

using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace L6.Infrastructure.Disruptor
{

    // base domain class holds the dipatching note domains can nest domains. 
    public class CommandDisruptorRouter : ICommandPublisher
    {
        ICommandPublisher dispatcher;
        IList<ICommandPublisher> domains;
        private DomainMultiCommandDispatcher commandDispatch;

        /// <summary>
        /// domains must be in a list where domain id is the index...
        /// </summary>
        /// <param name="domains"></param>
        public CommandDisruptorRouter(IList<ICommandPublisher> domains)
        {
            //commandHandlers = new List<ICommandHandler>();
            //commandBus = new CommandRouter() as ICommandBus;

            var handle1 = new DisruptorActionCommandEventHandler<CommandHolder>(new Action<CommandHolder>(ProcessCommand));

            commandDispatch = new DomainMultiCommandDispatcher(handle1);
            dispatcher = commandDispatch;
          //  Start(commandDispatch);// FIXME should be started ?
            this.domains = domains; 
        }

        public void Start()
        {
            Task.Factory.StartNew(() => commandDispatch.Start(), TaskCreationOptions.LongRunning);
        }



        public void ProcessCommand(CommandHolder command) 
        {
            IValidatableObject valObj = ((dynamic)command.Value) as IValidatableObject;

            if (valObj != null)
                valObj.Validate(null);
          

            //CommandRouter 
            if (command.Value.domainId == 0)
                throw new ArgumentException("Cant have 0 domain id when using router");

            domains[command.Value.domainId].Publish(command.Value); 
        }

        public ICommandPublisher Publisher
        {
            get { return dispatcher; }
        }

        public void Publish<T>(T args) where T : Command
        {
            if (args.domainId == 0 )
                throw new ArgumentException("Cant have 0 domain id when using router");
            dispatcher.Publish<T>(args); 
        }
    }
}

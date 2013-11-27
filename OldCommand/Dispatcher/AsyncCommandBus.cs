
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;
using L6.CommandProcessor.Command;


namespace L6.CommandProcessor.Dispatcher
{
    // must be multi threaded !
    public class AsyncCommandBus : IOldCommandBus
    {
        public void Submit<TCommand>(TCommand command) where TCommand : IOldCommand
        {
            var handler = DependencyResolver.Current.GetService<ICommandHandler<TCommand>>();
            if (!((handler != null) && handler is ICommandHandler<TCommand>))
            {
                throw new CommandHandlerNotFoundException(typeof(TCommand));
            }

            Task.Run(() => Schedule<TCommand>(command, handler));


        }

        private void Schedule<TCommand>(TCommand command, ICommandHandler<TCommand> handler) where TCommand : IOldCommand
        {
            try
            {
                handler.Execute(command);
            }
            catch (Exception)
            {
                ;// FIXME! fire event on bus and log 
            }
        }



        public IEnumerable<ValidationResult> Validate<TCommand>(TCommand command) where TCommand : IOldCommand
        {
            return HandleValidation<TCommand>(command);
        }

        private static IEnumerable<ValidationResult> HandleValidation<TCommand>(TCommand command) where TCommand : IOldCommand
        {
            var handler = DependencyResolver.Current.GetService<IValidationHandler<TCommand>>();
            if (handler != null && (handler is IValidationHandler<TCommand> ))
            {
                throw new ValidationHandlerNotFoundException(typeof(TCommand));
            }
            return handler.Validate(command);
        }
    }
}


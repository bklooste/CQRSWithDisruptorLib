
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using L6.CommandProcessor.Command;


namespace L6.CommandProcessor.Dispatcher
{
    public class DefaultCommandBus : IOldCommandBus
    {
        public void Submit<TCommand>(TCommand command) where TCommand: IOldCommand
        {    
            var handler = DependencyResolver.Current.GetService<ICommandHandler<TCommand>>();
            if (handler != null && (handler is ICommandHandler<TCommand> == false))
                throw new CommandHandlerNotFoundException(typeof(TCommand));

          handler.Execute(command);
 
        }
        public IEnumerable<ValidationResult> Validate<TCommand>(TCommand command) where TCommand : IOldCommand
        {
            var handler = DependencyResolver.Current.GetService<IValidationHandler<TCommand>>();
            if (handler != null && (handler is IValidationHandler<TCommand>) == false )
                throw new ValidationHandlerNotFoundException(typeof(TCommand));
  
            return handler.Validate(command);
        }
    }
}


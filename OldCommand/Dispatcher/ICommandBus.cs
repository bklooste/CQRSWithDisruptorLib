using L6.CommandProcessor.Command;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace L6.CommandProcessor.Dispatcher
{
    public interface IOldCommandBus
    {
        void Submit<TCommand>(TCommand command) where TCommand: IOldCommand;
        IEnumerable<ValidationResult> Validate<TCommand>(TCommand command) where TCommand : IOldCommand;
    }
}


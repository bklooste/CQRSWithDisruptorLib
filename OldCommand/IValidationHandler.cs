using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using L6.CommandProcessor;


namespace L6.CommandProcessor.Command
{
    public interface IValidationHandler<in TCommand> where TCommand : IOldCommand
    {
        IEnumerable<ValidationResult>  Validate(TCommand command);
    }
}

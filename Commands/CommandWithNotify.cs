using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.Commands
{
    public abstract class CommandWithNotify : DomainCommand
    {
      //  public abstract IEnumerable<ValidationResult> Validate();

        //remember needs to be long lived RX channel etc..
        Action<Guid,CommandState> CallBack { get; set; }

        public CommandWithNotify(Command com) : base ( com)
        {
        }
    }

    // creatws


      
}

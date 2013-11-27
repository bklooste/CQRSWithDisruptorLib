using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using L6.Infrastructure.Domain;
using L6.Infrastructure.Util;


namespace L6.Infrastructure.Commands
{

    public class DomainCommand : Command
    {
        public Command Command { get; private set; }
   //     public Action Callback { get; set; }
        public Guid ToDomain { get; set; } //to 
        //public Guid FromDomain { get; set; } 
        //public Guid Transaction { get; set; }
        //public Guid Id { get; set; }
//        Action<T> FailCallback { get; }
      
        //public override IEnumerable<ValidationResult> Validate()
        //{
        //    return Command.Validate(); 
        //}

        public DomainCommand(Command com)
        {
            this.Command = com; 
        }

    }

    // command that works on aggregate root of type T
    public abstract class PersistantCommand<T> : Command where T: IAggregateRoot<T>
    {

        // optional 
        public uint Id { get; set; }
        //public bool Completed { get; set; }
        //public bool  { get; set; }

     //   public override abstract IEnumerable<ValidationResult> Validate();
    }



  

   




}

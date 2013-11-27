using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using L6.Infrastructure.Domain;
using L6.Infrastructure.Events;
using L6.Infrastructure.Util;


namespace L6.Infrastructure.Commands
{

    public class EventCommand : Command
    {
        public DomainEvent Event { get; private set; }

        public EventCommand(DomainEvent com)
        {
            this.Event = com; 
        }

    }




  

   




}

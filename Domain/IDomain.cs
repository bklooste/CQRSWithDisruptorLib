using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L6.Infrastructure.Commands;

namespace L6.Infrastructure.Domain
{
    public interface IDomain
    {
        Guid Id {get;} 
        //DomainCommandRouter EventHandler { get; set; }
       ICommandPublisher Publisher { get; }  // send commands to domain
    }

}

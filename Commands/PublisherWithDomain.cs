using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disruptor;
using Disruptor.Dsl;
using L6.Infrastructure.Commands;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Threading;



namespace L6.Infrastructure.Commands
{
    public  class PublisherWithDomain : ICommandPublisher
    {

        ICommandPublisher bus;
        int domainId;
        public PublisherWithDomain(ICommandPublisher bus , int domain)
        {
            this.bus = bus;
            this.domainId = domain;

  
        }

       // [MethodImpl(MethodImplOptions.NoInlining)]
        public void Publish<T>(T command) where T: Command
        {
            command.domainId = domainId;
            bus.Publish<T>(command); 
         
        }








        public void Publish(Command args)
        {
            throw new NotImplementedException();
        }
    }
}

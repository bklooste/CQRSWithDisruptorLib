using System;
using L6.Infrastructure.Commands;


namespace L6.Infrastructure.Disruptor
{
    public interface IDomainCommandDispatcher :ICommandPublisher
    {
     //   int DomainId { get; }
     //   void Publish(Command command);
    }
}

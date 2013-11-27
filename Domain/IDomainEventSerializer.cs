using System;
using L6.Infrastructure.Events;
namespace L6.Infrastructure.Domain
{
    public interface IDomainEventSerializer
    {
        DomainEvent Deserialize(Type targetType, string serializedDomainEvent);
        string Serialize(DomainEvent domainEvent);
    }
}

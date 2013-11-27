using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using L6.Infrastructure.Events;

namespace L6.Infrastructure.Disruptor
{
    public class EventHolder
    {
        public DomainEvent Value { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.EventSourcing
{
    [Serializable]
    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException(string msg ) : base(msg) {}
        public AggregateNotFoundException(string msg , Exception ex) : base(msg ,ex) { }

    }
}

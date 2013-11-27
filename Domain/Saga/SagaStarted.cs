using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L6.Infrastructure.Events;

namespace L6.Infrastructure.Domain.Sagas
{
    [Serializable]
    public class SagaStarted :DomainEvent
    {

        public readonly string SagaType;

        public SagaStarted(SagaBase type)
        {
            SagaType = type.GetType().ToString();
        }
    }
}

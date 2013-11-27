using System;
using System.Collections.Generic;
using Autofac;


namespace L6.Infrastructure.Events
{


    public delegate void EventDelegate(DomainEvent s); //FIXME use instead of Delegate but work out to convert from Action<T>


    public class DomainEventRegistrationRemover : IDisposable
    {
        private readonly Action CallOnDispose;

        public DomainEventRegistrationRemover(Action ToCall)
        {
            this.CallOnDispose = ToCall;
        }


        public void Dispose()
        {
            this.CallOnDispose.DynamicInvoke();
        }
    }





}


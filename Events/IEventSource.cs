using System;
using System.Collections.Generic;
using Autofac;


namespace L6.Infrastructure.Events
{


    public interface IEventSource
    {
        void ClearAllCallbacks();
        void ClearCallbacks<T>();
        // prefered for short subs 
        IDisposable Subscribe<T>(Action<T> callback) where T : DomainEvent;
        // prefered for long subs 
        IDisposable SubscribeHandler(IHandles handler); 

        // extras
    //    IDisposable Subscribe(Type t, Delegate delegate1);
        IDisposable Subscribe(Type key, IHandles handler);

    }


}


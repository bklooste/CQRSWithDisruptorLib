using System;
namespace L6.Infrastructure.Events
{
    public interface IEventBus : IEventPublisher , IEventSource
    {
     //   void ClearAllCallbacks();
      //  void ClearCallbacks<T>();
      ////  void Publish<T>(T args) where T : IDomainEvent;
      //  IDisposable Subscribe(Type t, Delegate delegate1);
      //  IDisposable Subscribe<T>(Action<T> callback) where T : IDomainEvent;
    }
}

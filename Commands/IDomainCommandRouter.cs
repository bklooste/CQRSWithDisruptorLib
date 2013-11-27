using System;
namespace L6.Infrastructure.Commands
{
    public interface ICommandSource
    {
        void Register(ICommandHandler handler);
        void ClearCallback<T>() where T : Command; 
    }

    public interface ICommandPublisher 
    {

        void Publish<T>(T args) where T : Command;  
      //  void Publish(Command args); // runtime 

      
    } 

    //public interface ICommandPublisher<T> : ICommandPublisher 
    //    where T : Command
    //{

    //    void Publish(T args);
 
    //}

    public interface ICommandBus : ICommandPublisher, ICommandSource
    {

     

    }

}

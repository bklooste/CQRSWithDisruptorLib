using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.Commands
{
    //public interface ICommandHandler
    //{
    //}

    //public interface ICommandHandler<T> : ICommandHandler where T : IDomainMessage
    //{
    //}


    // for tests loggers etc 
    public class CommandHandler<T>:  ICommandHandler<T> where T: Command
    {
        Action<T>  callback;
        public CommandHandler(Action<T> callback)
        {
            this.callback =  callback;

        }
        public void Handle(T args)
        {
            callback(args);
        }

        public void HandleDispatch(Command args)
        {
            callback(args as T);
        }
    }


}

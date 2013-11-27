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
    public class ActionCommandHandler : ICommandHandler<ActionCommand> 
    {
        public ActionCommandHandler()
        {

        }
        public void Handle(ActionCommand args)
        {
            args.Action(); 
        }

        public void HandleDispatch(Command args)
        {
            Handle(args as ActionCommand);
        }
    }


}

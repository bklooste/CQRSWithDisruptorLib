using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using L6.Infrastructure.Domain;
using L6.Infrastructure.Events;
using L6.Infrastructure.Util;


namespace L6.Infrastructure.Commands
{
    // used for replies to Business Domain
    public class ActionCommand : Command 
    {
        public Action Action { get; private set; }

        public ActionCommand(Action com)
        {
            this.Action = com; 
        }


        //public ActionCommand(Expression<Action> com)
        //{
        //    this.Action = com.Compile();
        //}

    }
       

    //// special command  startup will have one handler for this which will invoke all ActionCommands.
    //public class ActionCommand<T> : ActionCommand
    //{
    //    public Action<T> Action { get; private set; }
    //    public T  Data { get; private set; }


    //    public ActionCommand(Action<T>  com , T data)
    //    {
    //        this.Action = com;
    //        this.Data = data; 
    //    }

    //}




  

   




}

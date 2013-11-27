using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L6.Data.Infrastructure;
using L6.Infrastructure.Commands;
using L6.Infrastructure.Events;

namespace L6.Infrastructure.Domain.Sagas
{
    public interface IPersistedSaga
    {
        void OnStateChanged(object sender , EventHandler handler);


        void SaveState(); 
    }
}

//        /// <summary>
//        /// This class is used to define sagas containing data and handling a message.
//        /// To handle more message types, implement <see cref="IMessageHandler{T}"/>
//        /// for the relevant types.
//        /// To signify that the receipt of a message should start this saga,
//        /// implement <see cref="ISagaStartedBy{T}"/> for the relevant message type.
//        /// </summary>
//        /// <typeparam name="T">A type that implements <see cref="ISagaEntity"/>.</typeparam>
//        public abstract class
//            ProcessManager<T,U> : Saga<U>   //FIXME use EventSourced not EventSOurcedAR , its just so we can use the repository classes
            
//            where U: DomainEvent
//            where T: EventSourced // can use seperate data or just persist the public members
       
//        {

//            IEventSource bus;
//            ICommandPublisher commandBus;

//            public ProcessManager(IEventSource bus, ICommandPublisher commandBus, IRepository<T> repository)
//            {
//                this.bus = bus;
//                this.commandBus = commandBus; 
//            }


          
//            // onlod from repository  ( recheck timeout , fire if passed else recreate



            
//        }
//    }



using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using L6.Infrastructure.Commands;
using L6.Infrastructure.Events;
using L6.Infrastructure.EventSourcing.Disruptor;

namespace L6.Infrastructure.Domain.Sagas
{

    //public class SagaData()
    //{

    //}


        [Serializable]
    public class SagaLauncher //:EventSourcedAR<SagaData>
    {
        IDictionary<Type, IStartedByEvent> sagaStartedByEventMapper = new Dictionary<Type, IStartedByEvent>();
        IDictionary<Type, IDomainRepository> sagaRepositories= new Dictionary<Type, IDomainRepository >();
        IDictionary<int, SagaBase> runningSagas = new Dictionary<int, SagaBase>();

         IEventSource bus;
         ICommandPublisher commandBus;

         public SagaLauncher(IEventSource bus, ICommandPublisher commandBus)
         {
             this.bus = bus;
             this.commandBus = commandBus;
         }

            /// <summary>
            /// Send a command to load all the sagas
            /// 
            /// Note we stope the main thread for this 
            /// </summary>
            /// <param name="dataBus"></param>
         void Init(ICommandPublisher dataBus)
         {
            if (sagaStartedByEventMapper.Count() == 0 ) 
            {
                Debug.WriteLine("Warning = No sagas loaded"); 
                return;
            }
             ManualResetEvent resetEvent = new ManualResetEvent(false);

             var command = new GetEventsForTypeStoreCommand() 
             {
                  type = "SagaLauncher",
                   callbackevents = new StoreCommandAction<IDictionary<int,IEnumerable<DomainEvent>>>( (x) =>
                   {
                        foreach( var saga in x)
                        {
                            if ( x.Values.Any( y => y is SagaCompleted))  // finished
                                continue;

                            Type type = Type.GetType (x.Values.OfType<SagaStarted>().First().SagaType);
                            SagaBase item = CreateFromRepository(type);
                            runningSagas.Add( saga.Key , item);

                            item.LoadFromHistoricalEvents( saga.Value);
                        }
                        resetEvent.Set();
                   } 
                   ) 
             };
             dataBus.Publish<GetEventsForTypeStoreCommand>(command);

             // Wait for finish.
             resetEvent.WaitOne(5000);


         }

         private SagaBase CreateFromRepository(Type type)
         {
             var sagaRepository = (dynamic)sagaRepositories[type];
             SagaBase item = (SagaBase)sagaRepository.New();
             return item;
         }



        //FIXME  we require all saga types to have a repository for new break this 
        public void AddSagaType( SagaBase saga , IDomainRepository repository)
        {

            sagaStartedByEventMapper.Add(saga.GetType(), (IStartedByEvent)saga);
            sagaRepositories.Add(saga.GetType(), repository);

        }

        public void AddSagaType(Type sagaType, IDomainRepository repository)
        {
            var saga = Activator.CreateInstance(sagaType);
           AddSagaType( (SagaBase) saga , repository);

        }

        // hooks event ..
        public void HookEvents(Events.DomainEvent x)
        {
        
            // see event is in sagaStarted and if so started a saga. 
            if ( sagaStartedByEventMapper.ContainsKey( x.GetType() )) //started by , note 1 Saga per event ! 
            {
    
                SagaBase saga = CreateFromRepository(sagaStartedByEventMapper[x.GetType()].GetType());

                var token = bus.SubscribeHandler(saga); // hook events
                saga.CommandBus = commandBus;
                saga.EventBusSubscriptionToken = token;
                saga.Start(x); 
               // if persitant save
                if (saga is IPersistedSaga)
                {
                    ((IPersistedSaga)saga).SaveState(); // Should be started .. FIXME .. now back to logic 
                    //SagaStarted @event = new SagaStarted(sagaInstance.GetType().ToString());
                }
               
            }
          

        }
    }
}

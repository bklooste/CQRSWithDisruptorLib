using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disruptor;
using Disruptor.Dsl;
using L6.Infrastructure.Commands;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Threading;
using L6.Infrastructure.Events;



namespace L6.Infrastructure.Disruptor
{
    public  class EventDispatcher : IEventPublisher
    {
        private RingBuffer<EventHolder> _ringBuffer;
        private int bufferSize = 1024 * 8;
        private Disruptor<EventHolder> _disruptor;

        IEventHandler<EventHolder>[] handlers;

        public EventDispatcher(params IEventHandler<EventHolder>[] handlers)
        {
            this.handlers = handlers; 


  
        }

        public void Start()
        {

           //// if ( multi)
           //     _disruptor = new Disruptor<EventHolder>(() => new EventHolder(),
           //                                new MultiThreadedClaimStrategy(bufferSize),
           //                                new YieldingWaitStrategy(),
           //                                TaskScheduler.Default);
           // //else
            _disruptor = new Disruptor<EventHolder>(() => new EventHolder(),
                                                      new SingleThreadedClaimStrategy(bufferSize),
                                                      new BlockingWaitStrategy(),
                                                      TaskScheduler.Default);



            _disruptor.HandleEventsWith(handlers);
            _ringBuffer = _disruptor.RingBuffer;
            _disruptor.Start();
         
        }


       // [MethodImpl(MethodImplOptions.NoInlining)]
        public void Publish<T>(T @event) where T: DomainEvent
        {
            Publish(@event);            
         
        }

        public void Publish(DomainEvent @event)
        {
            var sequence = _ringBuffer.Next();
            _ringBuffer[sequence].Value = @event;
            _ringBuffer.Publish(sequence);
        }
    }
}

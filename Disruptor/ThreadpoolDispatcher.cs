using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disruptor;
using Disruptor.Dsl;
using L6.Infrastructure.Commands;
using System.Runtime.CompilerServices;
using System.Threading;



namespace L6.Infrastructure.Disruptor
{
    public  class ThreadpoolDispatcher : ICommandPublisher
    {
        //private RingBuffer<CommandHolder> _ringBuffer;
        //private int bufferSize = 1024 * 8;
        //private Disruptor<CommandHolder> _disruptor;

        IEventHandler<CommandHolder>[] handlers;

        public ThreadpoolDispatcher(params IEventHandler<CommandHolder>[] handlers)
        {
            this.handlers = handlers; 


  
        }

        //public void Start(bool multi)
        //{

        //    //if ( multi)
        //    //    _disruptor = new Disruptor<CommandHolder>(() => new CommandHolder(),
        //    //                               new MultiThreadedLowContentionClaimStrategy(bufferSize),
        //    //                               new YieldingWaitStrategy(),
        //    //                               TaskScheduler.Default);
        //    //else
        //    //    _disruptor = new Disruptor<CommandHolder>(() => new CommandHolder(),
        //    //                                              new SingleThreadedClaimStrategy(bufferSize),
        //    //                                              new YieldingWaitStrategy(), 
        //    //                                              TaskScheduler.Default);



        //    //_disruptor.HandleEventsWith(handlers);
        //    //_ringBuffer = _disruptor.RingBuffer;
        //    //_disruptor.Start();
         
        //}


        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Publish<T>(T command) where T: Command
        {

            //ThreadPool.QueueUserWorkItem(new WaitCallback(command.), null);

            //var sequence = _ringBuffer.Next();
            //        _ringBuffer[sequence].Value = command;
            //        _ringBuffer.Publish(sequence);
        }








        public void Publish(Command args)
        {
            throw new NotImplementedException();
        }
    }
}

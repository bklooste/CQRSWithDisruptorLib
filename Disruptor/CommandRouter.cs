using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disruptor;
using Disruptor.Dsl;
using L6.Infrastructure.Commands;



namespace L6.Infrastructure.Disruptor
{
    public  class DomainCommandDispatcher : ICommandPublisher
    {
        private readonly RingBuffer<CommandHolder> _ringBuffer;
        private int bufferSize = 1024 * 8;
        private readonly Disruptor<CommandHolder> _disruptor;

        public DomainCommandDispatcher(params IEventHandler<CommandHolder>[] handlers)
        {
            _disruptor = new Disruptor<CommandHolder>(() => new CommandHolder(),
                                                          new SingleThreadedClaimStrategy(bufferSize),
                                                          new YieldingWaitStrategy(), 
                                                          TaskScheduler.Default);



            _disruptor.HandleEventsWith(handlers);    
            _ringBuffer = _disruptor.RingBuffer;
        }


        //[MethodImpl(MethodImplOptions.NoInlining)]
        public void Publish<T>(T command) where T: Command
        {

            var sequence = _ringBuffer.Next();
                    _ringBuffer[sequence].Value = command;
                    _ringBuffer.Publish(sequence);
        }








        public void Publish(Command args)
        {
            throw new NotImplementedException();
        }
    }
}

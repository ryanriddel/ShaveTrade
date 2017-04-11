using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Disruptor;
using Disruptor.Dsl;

namespace LOAMS
{
    public delegate void BufferEventHandlerFunction<T>(T data);
    

    public class FastBuffer<T> where T: class, IBufferItem<T>, new()
    {
        RingBuffer<T> _ringBuffer;
        Disruptor<T> _disruptor;
        bool _disruptorHasStarted = false;
        int _numExtraHandlersAdded = 0;
        int _bufferSize;

        List<BufferEventHandler<T>> _eventHandlers = new List<BufferEventHandler<T>>();

        EventPublisher<T> _publisher;

        public FastBuffer(int bufferSize = 1024)
        {
            _bufferSize = bufferSize;
            
            _disruptor = new Disruptor<T>(() => { return new T(); }, _bufferSize, TaskScheduler.Default, ProducerType.Single, new SleepingWaitStrategy());
            
        }

        public void ConsumerSubscribe(IMarketDataConsumer<T> cons)
        {
            BufferEventHandler<T> newHandler = new BufferEventHandler<T>();
            if (!_disruptorHasStarted)
            {
                newHandler.ThrowEvent += cons.NewDataHandler;
                _eventHandlers.Add(newHandler);
            }
            else
            {
                _eventHandlers[_numExtraHandlersAdded % BufferEventHandler<T>._numConsumers].ThrowEvent += cons.NewDataHandler;
                _numExtraHandlersAdded++;
            }
        }

        public void Add(T data)
        {

            long sequenceNum = _ringBuffer.Next();
            T temp = _ringBuffer[sequenceNum];
            temp.Update(data);
            _ringBuffer.Publish(sequenceNum);
            
        }


        public void Begin()
        {
            _disruptor.HandleEventsWith(_eventHandlers.ToArray<IEventHandler<T>>());
            _ringBuffer = _disruptor.Start();

            _publisher = new EventPublisher<T>(_ringBuffer);

            _disruptorHasStarted = true;
            
        }

        void publishEvent(T arg, long sequence, T arg2)
        {

        }


        class BufferEventHandler<A> : IEventHandler<A>
        {
            public static int _numConsumers = 0;
            private readonly int _ordinal;


            public event BufferEventHandlerFunction<A> ThrowEvent = delegate { };
            

            public BufferEventHandler()
            {
                this._ordinal = _numConsumers++;
            }

            public void OnNext(A data, long sequence, bool endOfBatch)
            {
                ThrowEvent(data);
            }

            public void OnEvent(A data, long sequence, bool endOfBatch)
            {
                ThrowEvent(data);
            }
        }

    }

    class EventTranslator<T> : IEventTranslator<T>
    {
        T _data;
        public EventTranslator(T data)
        {
            _data = data;
        }
        public void TranslateTo(T eventData, long sequence)
        {
            eventData = _data;
        }
    }

    
    public interface IBufferItem<T>
    {
        void Update(T newItem);
    }

}

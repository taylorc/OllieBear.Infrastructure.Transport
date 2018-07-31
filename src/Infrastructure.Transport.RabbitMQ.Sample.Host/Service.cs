using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Transport.Interfaces;

namespace Infrastructure.Transport.RabbitMQ.Sample.Host
{
    internal class Service : IService
    {
        private readonly IConsumer _consumer;
        private readonly IProducer _producer;

        public Service(
            IConsumer consumer,
            IProducer producer)
        {
            _consumer = consumer;
            _producer = producer;
        }

        public void Start()
        {
            for (var i = 0; i < 5; i++)
            {
                var testServiceBusCommand = new TestQueueCommand
                { 
                    SampleInt = 1,
                    SampleText = "Test Command",
                    SampleGuid = Guid.NewGuid(),
                    SampleDate = DateTime.Now
                };

                Console.WriteLine("Sending test command");

                var correlationId = _producer.Publish(testServiceBusCommand);

                Console.WriteLine($"Queue responded with correlation Id: [{correlationId}]");
            }

            Thread.Sleep(5000);

            _consumer.StartConsuming()
                .ContinueWith(
                    t => Console.WriteLine(t?.Exception?.ToString()),
                    TaskContinuationOptions.OnlyOnFaulted);
        }

        public void Stop()
        {
            _consumer.Dispose();
        }
    }
}

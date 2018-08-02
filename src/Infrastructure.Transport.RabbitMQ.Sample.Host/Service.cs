using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Transport.Interfaces;
using Infrastructure.Transport.RabbitMQ.Sample.Host.Definitions;

namespace Infrastructure.Transport.RabbitMQ.Sample.Host
{
    internal class Service : IService
    {
        private readonly ITopology _topology;

        public Service(ITopology topology)
        {
            _topology = topology;
        }

        public void Start()
        {
            var blueProducer = _topology.GetProducer("TestQueueFirst");
            var greenProducer = _topology.GetProducer("TestQueueSecond");

            blueProducer.Publish(new RedCommand());

            for (var i = 0; i < 5; i++)
            {
                var testServiceBusCommand = new BlueCommand
                { 
                    SampleInt = 3,
                    SampleDateTime = DateTime.Now
                };

                blueProducer.Publish(testServiceBusCommand);
            }
            
            for (var i = 0; i < 2; i++)
            {
                var testServiceBusCommand = new GreenCommand
                {
                    SampleGuid = Guid.NewGuid(),
                    SampleString = "Test String"
                };

                greenProducer.Publish(testServiceBusCommand);
            }

            Thread.Sleep(2000);

            foreach (var consumer in _topology.GetConsumers())
            {
                consumer.StartConsuming()
                    .ContinueWith(
                        t => Console.WriteLine(t?.Exception?.ToString()),
                        TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        public void Stop()
        {
        }
    }
}

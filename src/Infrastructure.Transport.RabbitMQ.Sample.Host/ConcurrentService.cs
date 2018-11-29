using System;
using System.Threading;
using Infrastructure.Logging;
using Infrastructure.Transport.Interfaces;
using Infrastructure.Transport.RabbitMQ.Sample.Host.Definitions;

namespace Infrastructure.Transport.RabbitMQ.Sample.Host
{
    public class ConcurrentService : IConcurrentService
    {
        private readonly ITopology _topology;
        private readonly ILog _logger;

        public ConcurrentService(
            ITopology topology, 
            ILog logger)
        {
            _topology = topology;
            _logger = logger;
        }

        public void ResolveProducersConcurrently()
        {
            for (var i = 0; i < 100; i++)
            {
                CreateResolveTask(i);
            }
        }

        private void CreateResolveTask(int counter)
        {
            _logger.Debug($"Resolving queue #{counter}");
            var queue = counter % 2 == 0 ? "TestQueueThird" : "TestQueueFourth";

            var thread = new Thread(ResolveProducer);
            thread.Start(new Tuple<ITopology, string>(_topology, queue));
        }

        private static void ResolveProducer(object tuple)
        {
            var values = (Tuple<ITopology, string>)tuple;

            var producer = values.Item1.GetProducer(values.Item2);

            var testServiceBusCommand = new YellowCommand
            {
                SampleGuid = Guid.NewGuid(),
                SampleString = "Concurrently Produced (YELLOW) String"
            };

            producer.Publish(testServiceBusCommand);
        }
    }
}

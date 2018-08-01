using System;
using System.Collections.Generic;
using Infrastructure.Transport.Interfaces;
using Infrastructure.Transport.Interfaces.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Transport.RabbitMQ
{
    public class Topology : ITopology, IDisposable
    {
        private readonly IDictionary<string, IConsumer> _consumers;
        private readonly IDictionary<string, IProducer> _producers;

        public Topology(
            Func<QueueConfigurationOptions, IConsumer> consumerFunc,
            Func<QueueConfigurationOptions, IProducer> producerFunc,
            IOptions<TransportConfigurationOptions> transportConfigurationOptionsAccessor
            )
        {
            var transportConfigurationOptions = transportConfigurationOptionsAccessor.Value;

            _consumers = new Dictionary<string, IConsumer>();
            _producers = new Dictionary<string, IProducer>();

            foreach (var queue in transportConfigurationOptions.ConsumerQueues ?? new List<QueueConfigurationItem>())
            {
                var options = queue.Options;
                _consumers.Add(queue.Key, consumerFunc(options));
            }

            foreach (var queue in transportConfigurationOptions.ProducerQueues ?? new List<QueueConfigurationItem>())
            {
                var options = queue.Options;
                _producers.Add(queue.Key, producerFunc(options));
            }
        }

        public IConsumer GetConsumer(string key) => _consumers[key];

        public IProducer GetProducer(string key) => _producers[key];

        public IEnumerable<IConsumer> GetConsumers() => _consumers.Values;

        public IEnumerable<IProducer> GetProducers() => _producers.Values;

        public void Dispose()
        {
            foreach (var consumer in _consumers.Values)
            {
                consumer?.Dispose();
            }

            foreach (var producer in _producers.Values)
            {
                producer?.Dispose();
            }
        }
    }
}

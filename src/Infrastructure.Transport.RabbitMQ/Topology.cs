using System;
using System.Collections.Generic;
using Infrastructure.Logging;
using Infrastructure.Serialization.Interfaces;
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
            IMessageHandler messageHandler,
            IChannelFactory channelFactory,
            ISerializer serializer,
            ILog logger,
            IOptions<TransportConfigurationOptions> transportConfigurationOptionsAccessor
            )
        {
            var transportConfigurationOptions = transportConfigurationOptionsAccessor.Value;

            _consumers = new Dictionary<string, IConsumer>();
            _producers = new Dictionary<string, IProducer>();

            foreach (var item in transportConfigurationOptions.ConsumerQueues)
            {
                var options = item.Options;
                _consumers.Add(item.Key, new Consumer(messageHandler, serializer, logger, channelFactory, options));
            }

            foreach (var item in transportConfigurationOptions.ProducerQueues)
            {
                var options = item.Options;
                _producers.Add(item.Key, new Producer(serializer, logger, channelFactory, options));
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

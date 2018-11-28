using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Infrastructure.Transport.Interfaces;
using Infrastructure.Transport.Interfaces.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Transport.RabbitMQ
{
    public class Topology : ITopology, IDisposable
    {
        private readonly IDictionary<string, IConsumer> _consumers;
        private readonly IDictionary<string, IProducer> _producers;

        private readonly Func<QueueConfigurationOptions, IConsumer> _consumerFunc;
        private readonly Func<QueueConfigurationOptions, IProducer> _producerFunc;

        private readonly TransportConfigurationOptions _options;

        public Topology(
            Func<QueueConfigurationOptions, IConsumer> consumerFunc,
            Func<QueueConfigurationOptions, IProducer> producerFunc,
            IOptions<TransportConfigurationOptions> transportConfigurationOptionsAccessor)
        {
            _consumers = new ConcurrentDictionary<string, IConsumer>();
            _producers = new ConcurrentDictionary<string, IProducer>();

            _consumerFunc = consumerFunc;
            _producerFunc = producerFunc;

            _options = transportConfigurationOptionsAccessor.Value;
        }
        
        public IConsumer GetConsumer(string key) => _consumers.ContainsKey(key) ? _consumers[key] : RegisterConsumer(key);

        public IProducer GetProducer(string key) => _producers.ContainsKey(key) ? _producers[key] : RegisterProducer(key);

        public IEnumerable<IConsumer> GetConsumers() => ConsumerKeys?.Select(GetConsumer) ?? new List<IConsumer>();

        public IEnumerable<IProducer> GetProducers() => ProducerKeys?.Select(GetProducer) ?? new List<IProducer>();

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

        private IEnumerable<string> ConsumerKeys => _options.ConsumerQueues?.Select(q => q.Key);

        private IEnumerable<string> ProducerKeys => _options.ProducerQueues?.Select(q => q.Key);

        private IConsumer RegisterConsumer(string key)
        {
            lock (_consumers)
            {
                if (_consumers.TryGetValue(key, out var consumer))
                    return consumer;

                var configuration =
                    _options
                        .ConsumerQueues?
                        .FirstOrDefault(q => q.Key == key);

                if (configuration == null)
                    throw new Exception($"No consumer queue configuration found in appsettings with key [{key}]");

                consumer = _consumerFunc(configuration.Options);

                _consumers.Add(key, consumer);

                return consumer;
            }
        }

        private IProducer RegisterProducer(string key)
        {
            lock (_producers)
            {
                if (_producers.TryGetValue(key, out var producer))
                    return producer;

                var configuration =
                    _options
                        .ProducerQueues?
                        .FirstOrDefault(q => q.Key == key);

                if (configuration == null)
                    throw new Exception($"No producer queue configuration found in appsettings with key [{key}]");

                producer = _producerFunc(configuration.Options);

                _producers.Add(key, producer);

                return producer;
            }
        }
    }
}

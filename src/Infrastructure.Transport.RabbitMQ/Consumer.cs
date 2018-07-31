using System;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Logging;
using Infrastructure.Serialization.Interfaces;
using Infrastructure.Transport.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Transport.RabbitMQ
{
    public class Consumer : IConsumer, IDisposable
    {
        private readonly IMessageHandler _messageHandler;
        private readonly ISerializer _serializer;
        private readonly ILog _logger;
        private readonly TransportConfigurationOptions _transportConfigurationOptions;

        private readonly IModel _channel;

        public Consumer(
            IMessageHandler messageHandler,
            ISerializer serializer,
            ILog logger,
            IChannelFactory channelFactory,
            TransportConfigurationOptions transportConfigurationOptions)
        {
            _messageHandler = messageHandler;
            _serializer = serializer;
            _logger = logger;
            _transportConfigurationOptions = transportConfigurationOptions;
            _channel = channelFactory.CreateChannel();
        }

        public Task StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var properties = ea.BasicProperties;

                var message = Encoding.UTF8.GetString(body);
                _logger.Info($"[CorrelationId={properties.CorrelationId}] " +
                             $"[Queue={_transportConfigurationOptions.QueueName}] " +
                             $"[MessageText=Message Received:{message}]");

                var deserializedType = Type.GetType(properties.ContentType);

                if (deserializedType == null)
                    throw new Exception($"Message type [{properties.ContentType}] not recognised by consumer");

                var deserializedObject = _serializer.DeserializeToType(body, deserializedType);

                _messageHandler.Handle(deserializedObject, deserializedType);
            };

            return Task.Run(() =>
            {
                _channel.BasicConsume(
                    queue: _transportConfigurationOptions.QueueName,
                    autoAck: true,
                    consumer: consumer);
            });
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
        }
    }
}

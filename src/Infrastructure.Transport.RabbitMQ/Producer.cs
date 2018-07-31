using System;
using System.Text;
using Infrastructure.Logging;
using Infrastructure.Logging.Extensions;
using Infrastructure.Serialization.Interfaces;
using Infrastructure.Transport.Interfaces;
using Infrastructure.Transport.Interfaces.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace Infrastructure.Transport.RabbitMQ
{
    public class Producer : IProducer
    {
        private readonly ISerializer _serializer;
        private readonly ILog _logger;
        private readonly QueueConfigurationOptions _queueConfigurationOptions;
        private readonly IModel _channel;

        public Producer(
            ISerializer serializer,
            ILog logger,
            IChannelFactory channelFactory,
            QueueConfigurationOptions queueConfigurationOptions)
        {
            _serializer = serializer;
            _logger = logger;
            _queueConfigurationOptions = queueConfigurationOptions;
            _channel = channelFactory.CreateChannel(queueConfigurationOptions);
        }

        public string Publish<T>(T msg)
        {
            try
            {
                var correlationId = Guid.NewGuid().ToString();
                var messageBody = _serializer.ToPayload(msg);

                var basicProperties = new BasicProperties
                {
                    CorrelationId = correlationId,
                    ContentType = typeof(T).AssemblyQualifiedName
                };

                _logger.Info($"[CorrelationId={correlationId}] " +
                             $"[Queue={_queueConfigurationOptions.QueueName}] " +
                             $"[MessageText=Message Sent:{Encoding.Default.GetString(messageBody)}]");

                _channel.BasicPublish(
                    exchange: "",
                    routingKey: _queueConfigurationOptions.QueueName,
                    basicProperties: basicProperties,
                    body: messageBody);

                return correlationId;
            }
            catch (Exception e)
            {
                _logger.Error($"Error publishing message: {e.DeepException()}");

                throw;
            }
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
        }
    }
}

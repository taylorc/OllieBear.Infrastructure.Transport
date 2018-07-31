using System;
using Infrastructure.Logging;
using Infrastructure.Logging.Extensions;
using Infrastructure.Serialization.Interfaces;
using Infrastructure.Transport.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace Infrastructure.Transport.RabbitMQ
{
    public class Producer : IProducer, IDisposable
    {
        private readonly ISerializer _serializer;
        private readonly ILog _logger;
        private readonly TransportConfigurationOptions _transportConfigurationOptions;

        private readonly IModel _channel;

        public Producer(
            ISerializer serializer, 
            ILog logger, 
            IChannelFactory channelFactory, 
            TransportConfigurationOptions transportConfigurationOptions)
        {
            _serializer = serializer;
            _logger = logger;
            _transportConfigurationOptions = transportConfigurationOptions;
            _channel = channelFactory.CreateChannel();
        }

        public string Publish<T>(T msg)
        {
            try
            {
                var correlationId = Guid.NewGuid().ToString();
                var messageBody = _serializer.ToPayload(msg);

                if (messageBody.Length > _transportConfigurationOptions.MaxMessageSize)
                {
                    _logger.Error($"Cannot send message; message size {messageBody.Length} bytes exceeds configured maximum {_transportConfigurationOptions.MaxMessageSize} bytes");
                    return Guid.Empty.ToString();
                }

                var basicProperties = new BasicProperties
                {
                    CorrelationId = correlationId,
                    ContentType = typeof(T).AssemblyQualifiedName
                };

                _channel.BasicPublish(
                    exchange: "",
                    routingKey: "",
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

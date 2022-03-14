using System;
using System.Collections.Generic;
using System.Text;
using Azure.Messaging.ServiceBus;
using Infrastructure.Logging;
using Infrastructure.Logging.Extensions;
using Infrastructure.Serialization.Interfaces;
using Infrastructure.Transport.Interfaces;
using Infrastructure.Transport.Interfaces.Options;

namespace Infrastructure.Transport.AzureServiceBus
{
    public class Producer : IProducer
    {
        private readonly ISerializer _serializer;
        private readonly ILog _logger;
        private readonly QueueConfigurationOptions _queueConfigurationOptions;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusSender _producer;

        public Producer(
            ISerializer serializer,
            ILog logger,
            IChannelFactory channelFactory,
            QueueConfigurationOptions queueConfigurationOptions)
        {
            _serializer = serializer;
            _logger = logger;
            _queueConfigurationOptions = queueConfigurationOptions;
            _serviceBusClient = channelFactory.CreateChannel();
            _producer = _serviceBusClient.CreateSender(_queueConfigurationOptions.QueueName);
        }

        public string Publish<T>(T msg)
        {
            try
            {
                var correlationId = Guid.NewGuid().ToString();
                var messageBody = _serializer.ToPayload(msg);

                var message = new ServiceBusMessage
                {
                    Body = new BinaryData(messageBody),
                    CorrelationId = correlationId
                };

                message.ApplicationProperties.Add("ContentType", typeof(T).AssemblyQualifiedName);

                _logger.Info($"[CorrelationId={correlationId}] " +
                             $"[Queue={_queueConfigurationOptions.QueueName}] " +
                             $"[MessageText=Message Sent:{Encoding.Default.GetString(messageBody)}]");

                _producer.SendMessageAsync(message);


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

            _producer.DisposeAsync();
            _serviceBusClient.DisposeAsync();
        }
    }
}

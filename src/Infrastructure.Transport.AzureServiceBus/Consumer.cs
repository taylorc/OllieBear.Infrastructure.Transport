using System;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Infrastructure.Logging;
using Infrastructure.Serialization.Interfaces;
using Infrastructure.Transport.Interfaces;
using Infrastructure.Transport.Interfaces.Options;
using Microsoft.Azure.Amqp.Framing;

namespace Infrastructure.Transport.AzureServiceBus
{
    public class Consumer : IConsumer
    {
        private readonly IMessageHandler _messageHandler;
        private readonly ISerializer _serializer;
        private readonly ILog _logger;
        private readonly QueueConfigurationOptions _queueConfigurationOptions;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusProcessor _consumer;

        public Consumer(
            IMessageHandler messageHandler,
            ISerializer serializer,
            ILog logger,
            IChannelFactory channelFactory,
            QueueConfigurationOptions queueConfigurationOptions)
        {
            _messageHandler = messageHandler;
            _serializer = serializer;
            _logger = logger;
            _queueConfigurationOptions = queueConfigurationOptions;
            _serviceBusClient = channelFactory.CreateChannel();
            _consumer = _serviceBusClient.CreateProcessor(_queueConfigurationOptions.QueueName);
        }

        public Task StartConsuming()
        {

            _consumer.ProcessMessageAsync += async (ea) =>
            {
                try
                {
                    var payload = ea.Message;
                    var properties = ea.Message.ApplicationProperties;

                    if (!properties.ContainsKey("ContentType") )
                        throw new TransportAzureServiceBusException(
                            "The message must have a content type in the Application Properties. Key ContentType is missing");

                    var contentType = properties["ContentType"].ToString();

                    if (string.IsNullOrEmpty(contentType))
                    {
                        throw new TransportAzureServiceBusException(
                            "The message must have a content type in the Application Properties. Value for Key ContentType is null");
                    }


                    _logger.Info($"[CorrelationId={ea.Message.CorrelationId}] " +
                                 $"[Queue={_queueConfigurationOptions.QueueName}] " +
                                 $"[MessageText=Message Received:{ea.Message}]");

                    var deserializedType = Type.GetType(contentType);

                    if (deserializedType == null)
                        throw new Exception($"Message type [{contentType}] not recognised by consumer");

                    var deserializedObject = _serializer.DeserializeToType(ea.Message.Body.ToArray(), deserializedType);

                    _messageHandler.Handle(deserializedObject, deserializedType);
                }
                catch (Exception e)
                {
                    _messageHandler.HandleException(e);
                }

                await ea.CompleteMessageAsync(ea.Message);
            };

            _consumer.ProcessErrorAsync += (ea) =>
            {
                _logger.Error($"[Exception={ea.Exception}] " +
                              $"[Queue={_queueConfigurationOptions.QueueName}] " +
                              $"[EntityPath={ea.EntityPath}] " +
                              $"[FullyQualifiedNamespace={ea.FullyQualifiedNamespace}] " +
                              $"[ErrorSource={ea.ErrorSource}]");

                return Task.CompletedTask;
            };


            return Task.Run(() =>
            {
                _consumer.StartProcessingAsync();
            });
        }

        public void Dispose()
        {
            _consumer.StopProcessingAsync();

            _consumer.DisposeAsync();
            _serviceBusClient.DisposeAsync();
        }
    }
}

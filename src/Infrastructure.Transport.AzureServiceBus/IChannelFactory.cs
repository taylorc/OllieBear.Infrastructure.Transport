using Azure.Messaging.ServiceBus;

namespace Infrastructure.Transport.AzureServiceBus
{
    public interface IChannelFactory
    {
        ServiceBusClient CreateChannel();
    }
}

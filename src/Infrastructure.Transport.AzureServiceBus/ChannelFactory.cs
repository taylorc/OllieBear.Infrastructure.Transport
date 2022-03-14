using Azure.Messaging.ServiceBus;
using Infrastructure.Transport.Interfaces.Options;
using Microsoft.Extensions.Options;


namespace Infrastructure.Transport.AzureServiceBus
{
    public class ChannelFactory : IChannelFactory
    {
        private readonly ServiceBusClient _serviceBusClient;

        public ChannelFactory(IOptions<TransportConfigurationOptions> transportConfigurationOptionsAccessor)
        {
            var transportConfigurationOptions = transportConfigurationOptionsAccessor.Value;
            _serviceBusClient = new ServiceBusClient(transportConfigurationOptions.HostName);
        }

        public ServiceBusClient CreateChannel()
        {

            return _serviceBusClient;
        }
    }
}

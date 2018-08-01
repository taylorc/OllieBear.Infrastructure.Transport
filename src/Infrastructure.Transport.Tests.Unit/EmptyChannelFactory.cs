using Infrastructure.Transport.Interfaces.Options;
using Infrastructure.Transport.RabbitMQ;
using RabbitMQ.Client;

namespace Infrastructure.Transport.Tests.Unit
{
    internal class EmptyChannelFactory : IChannelFactory
    {
        public IModel CreateChannel(QueueConfigurationOptions options)
        {
            return null;
        }
    }
}

using Infrastructure.Transport.Interfaces.Options;
using RabbitMQ.Client;

namespace Infrastructure.Transport.RabbitMQ
{
    public interface IChannelFactory
    {
        IModel CreateChannel(QueueConfigurationOptions options);
    }
}

using RabbitMQ.Client;

namespace Infrastructure.Transport.RabbitMQ
{
    public interface IChannelFactory
    {
        IModel CreateChannel();
    }
}

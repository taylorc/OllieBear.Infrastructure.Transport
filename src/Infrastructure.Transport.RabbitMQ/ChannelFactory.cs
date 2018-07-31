using Infrastructure.Transport.Interfaces;
using RabbitMQ.Client;

namespace Infrastructure.Transport.RabbitMQ
{
    public class ChannelFactory : IChannelFactory
    {
        private readonly TransportConfigurationOptions _transportConfigurationOptions;

        public ChannelFactory(TransportConfigurationOptions transportConfigurationOptions)
        {
            _transportConfigurationOptions = transportConfigurationOptions;
        }

        public IModel CreateChannel()
        {
            var factory = new ConnectionFactory { HostName = _transportConfigurationOptions.HostName };
            using (var connection = factory.CreateConnection())
            {
                var channel = connection.CreateModel();
                channel.QueueDeclare(
                    queue: _transportConfigurationOptions.QueueName,
                    durable: _transportConfigurationOptions.Durable,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                return channel;
            }
        }
    }
}

using Infrastructure.Transport.Interfaces.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Infrastructure.Transport.RabbitMQ
{
    public class ChannelFactory : IChannelFactory
    {
        private readonly ConnectionFactory _factory;

        public ChannelFactory(IOptions<TransportConfigurationOptions> transportConfigurationOptionsAccessor)
        {
            var transportConfigurationOptions = transportConfigurationOptionsAccessor.Value;
            _factory = new ConnectionFactory
            {
                HostName = transportConfigurationOptions.HostName,
                Password = transportConfigurationOptions.Password,
                VirtualHost = transportConfigurationOptions.VirtualHostName,
                UserName = transportConfigurationOptions.UserName,
                AutomaticRecoveryEnabled = true
            };
        }

        public IModel CreateChannel(QueueConfigurationOptions options)
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: options.QueueName,
                durable: options.Durable,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            return channel;
        }
    }
}

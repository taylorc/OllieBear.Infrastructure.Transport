using Infrastructure.Transport.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Infrastructure.Transport.RabbitMQ
{
    public class ChannelFactory : IChannelFactory
    {
        private readonly TransportConfigurationOptions _transportConfigurationOptions;
        private readonly ConnectionFactory _factory;

        public ChannelFactory(IOptions<TransportConfigurationOptions> transportConfigurationOptionsAccessor)
        {
            _transportConfigurationOptions = transportConfigurationOptionsAccessor.Value;
            _factory = new ConnectionFactory
            {
                HostName = _transportConfigurationOptions.HostName,
                Password = _transportConfigurationOptions.Password,
                VirtualHost = _transportConfigurationOptions.VirtualHostName,
                UserName = _transportConfigurationOptions.UserName,
                AutomaticRecoveryEnabled = true
            };
        }

        public IModel CreateChannel()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: _transportConfigurationOptions.QueueName,
                durable: _transportConfigurationOptions.Durable,
                exclusive: false,
                autoDelete: true,
                arguments: null);

            return channel;
        }
    }
}

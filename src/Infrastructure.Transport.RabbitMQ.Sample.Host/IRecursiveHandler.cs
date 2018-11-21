using Infrastructure.Transport.RabbitMQ.Sample.Host.Definitions;

namespace Infrastructure.Transport.RabbitMQ.Sample.Host
{
    public interface IRecursiveHandler
    {
        void Handle(YellowCommand command);
    }
}

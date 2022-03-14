using Infrastructure.Transport.AzureServiceBus.Sample.Host.Definitions;

namespace Infrastructure.Transport.AzureServiceBus.Sample.Host
{
    public interface IRecursiveHandler
    {
        void Handle(YellowCommand command);
    }
}

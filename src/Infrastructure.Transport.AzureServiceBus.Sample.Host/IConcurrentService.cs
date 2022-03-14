namespace Infrastructure.Transport.AzureServiceBus.Sample.Host
{
    public interface IConcurrentService
    {
        void ResolveProducersConcurrently();
    }
}

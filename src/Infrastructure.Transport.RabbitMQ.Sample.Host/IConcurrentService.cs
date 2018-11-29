namespace Infrastructure.Transport.RabbitMQ.Sample.Host
{
    public interface IConcurrentService
    {
        void ResolveProducersConcurrently();
    }
}

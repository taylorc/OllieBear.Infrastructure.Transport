namespace Infrastructure.Transport.Interfaces
{
    public interface IConnectionOptions
    {
        string QueueManagerName { get; set; }

        string QueueName { get; set; }

        int Options { get; set; }
    }
}

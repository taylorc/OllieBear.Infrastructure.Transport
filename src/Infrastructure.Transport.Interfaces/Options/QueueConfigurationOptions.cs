namespace Infrastructure.Transport.Interfaces.Options
{
    public class QueueConfigurationOptions
    {
        public string QueueName { get; set; }

        public bool Durable { get; set; }
    }
}

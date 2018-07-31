namespace Infrastructure.Transport.Interfaces
{
    public class TransportConfigurationOptions
    {
        public string HostName { get; set; }

        public string QueueName { get; set; }

        public bool Durable { get; set; }

        public long MaxMessageSize { get; set; }
    }
}

namespace Infrastructure.Transport.Interfaces
{
    public class TransportConfigurationOptions
    {
        public string HostName { get; set; }

        public string QueueName { get; set; }

        public bool Durable { get; set; }

        public long MaxMessageSize { get; set; }

        public string VirtualHostName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}

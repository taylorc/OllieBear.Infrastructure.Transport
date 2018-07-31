using System.Collections.Generic;

namespace Infrastructure.Transport.Interfaces.Options
{
    public class TransportConfigurationOptions
    {
        public string HostName { get; set; }

        public long MaxMessageSize { get; set; }

        public string VirtualHostName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public IEnumerable<QueueConfigurationItem> ConsumerQueues { get; set; }

        public IEnumerable<QueueConfigurationItem> ProducerQueues { get; set; }
    }
}

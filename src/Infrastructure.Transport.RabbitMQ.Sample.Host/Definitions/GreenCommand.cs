using System;

namespace Infrastructure.Transport.RabbitMQ.Sample.Host.Definitions
{
    public class GreenCommand
    {
        public Guid SampleGuid { get; set; }

        public string SampleString { get; set; }
    }
}

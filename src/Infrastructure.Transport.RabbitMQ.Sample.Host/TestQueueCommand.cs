using System;

namespace Infrastructure.Transport.RabbitMQ.Sample.Host
{
    public class TestQueueCommand
    {
        public Guid SampleGuid { get; set; }

        public string SampleText { get; set; }

        public int SampleInt { get; set; }

        public DateTime SampleDate { get; set; }
    }
}

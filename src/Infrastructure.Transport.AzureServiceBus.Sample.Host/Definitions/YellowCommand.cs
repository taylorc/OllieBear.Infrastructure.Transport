using System;

namespace Infrastructure.Transport.AzureServiceBus.Sample.Host.Definitions
{
    public class YellowCommand
    {
        public Guid SampleGuid { get; set; }

        public string SampleString { get; set; }
    }
}

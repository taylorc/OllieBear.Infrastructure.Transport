using System;
using Infrastructure.Transport.Interfaces;
using Infrastructure.Transport.RabbitMQ.Sample.Host.Definitions;

namespace Infrastructure.Transport.RabbitMQ.Sample.Host
{
    internal class RecursiveHandler : IRecursiveHandler
    {
        private readonly IProducer _producer;

        public RecursiveHandler(ITopology topology)
        {
            _producer = topology.GetProducer("TestQueueSecond");
        }

        public void Handle(YellowCommand command)
        {
            var greenCommand = new GreenCommand()
            {
                SampleGuid = Guid.NewGuid(),
                SampleString = "This message has been published from inside a handler."
            };

            _producer.Publish(greenCommand);
        }
    }
}

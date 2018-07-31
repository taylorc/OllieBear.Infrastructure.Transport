using System;
using Infrastructure.Transport.Interfaces;

namespace Infrastructure.Transport.RabbitMQ.Sample.Host
{
    internal class MessageHandler : IMessageHandler
    {
        public void Handle(object msg, Type type)
        {
            var queueCommand = msg as TestQueueCommand;

            if (queueCommand == null)
                return;

            var command = queueCommand;

            Console.WriteLine("Message received:");

            Console.WriteLine($"{command.SampleInt}, {command.SampleText}, {command.SampleGuid}");
        }

        public void HandleException(Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}

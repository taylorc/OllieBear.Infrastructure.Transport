using System;
using Infrastructure.Logging;
using Infrastructure.Logging.Extensions;
using Infrastructure.Transport.Interfaces;
using Infrastructure.Transport.RabbitMQ.Sample.Host.Definitions;

namespace Infrastructure.Transport.RabbitMQ.Sample.Host
{
    internal class MessageHandler : IMessageHandler
    {
        public void Handle(object msg, Type type)
        {
            var queueCommand = msg ;

            if (queueCommand is BlueCommand)
            {
                var command = (BlueCommand)queueCommand;
                
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Message received: {command.SampleInt}, {command.SampleDateTime}");
            }

            if (queueCommand is GreenCommand)
            {
                var command = (GreenCommand)queueCommand;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Message received: {command.SampleGuid}, {command.SampleString}");
            }
        }

        public void HandleException(Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(exception.DeepException());
        }
    }
}

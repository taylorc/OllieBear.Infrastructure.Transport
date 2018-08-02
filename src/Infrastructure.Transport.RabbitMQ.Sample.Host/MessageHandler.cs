using System;
using Infrastructure.Logging.Extensions;
using Infrastructure.Transport.Interfaces;
using Infrastructure.Transport.RabbitMQ.Sample.Host.Definitions;

namespace Infrastructure.Transport.RabbitMQ.Sample.Host
{
    internal class MessageHandler : IMessageHandler
    {
        public void Handle(object msg, Type type)
        {
            if (msg is RedCommand)
            {
                throw new Exception("Red command throws exception");
            }

            if (msg is BlueCommand)
            {
                var command = (BlueCommand)msg;
                
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Message received: {command.SampleInt}, {command.SampleDateTime}");
            }

            if (msg is GreenCommand)
            {
                var command = (GreenCommand)msg;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Message received: {command.SampleGuid}, {command.SampleString}");
            }
        }

        public void HandleException(Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(exception.DeepException());
        }
    }
}

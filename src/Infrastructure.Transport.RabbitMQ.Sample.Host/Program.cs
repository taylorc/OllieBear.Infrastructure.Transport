using System;
using System.IO;
using Infrastructure.Logging.Serilog.DependencyInjection;
using Infrastructure.Serialization;
using Infrastructure.Serialization.Interfaces;
using Infrastructure.Transport.Interfaces;
using Infrastructure.Transport.Interfaces.Options;
using Infrastructure.Transport.RabbitMQ.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Transport.RabbitMQ.Sample.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var services = new ServiceCollection();

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = configurationBuilder.Build();

            services
                .Configure<TransportConfigurationOptions>(configuration.GetSection("TransportConfigurationOptions"));

            services
                .AddSerilogLogging();

            services
                .AddTransient<IService, Service>();

            services
                .AddTransient<ISerializer, SerializerJson>();

            services
                .AddSingleton<IMessageHandler, MessageHandler>()
                .AddSingleton<IRecursiveHandler, RecursiveHandler>();

            services
                .AddRabbitMqService();

            var serviceProvider = services.BuildServiceProvider();

            var service = serviceProvider.GetService<IService>();

            service.Start();

            Console.ReadKey();
        }
    }
}

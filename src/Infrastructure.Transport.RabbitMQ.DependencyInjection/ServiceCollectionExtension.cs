using Infrastructure.Transport.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Transport.RabbitMQ.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRabbitMqService(this IServiceCollection services)
        {
            services.AddTransient<IChannelFactory, ChannelFactory>();

            services.AddTransient<IConsumer, Consumer>();

            services.AddTransient<IProducer, Producer>();

            return services;
        }
    }
}
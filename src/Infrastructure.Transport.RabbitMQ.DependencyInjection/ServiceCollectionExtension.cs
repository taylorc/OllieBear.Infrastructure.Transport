using Infrastructure.Transport.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Transport.RabbitMQ.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRabbitMqService(this IServiceCollection services)
        {
            services.AddTransient<IChannelFactory, ChannelFactory>();

            services.AddSingleton<ITopology, Topology>();

            return services;
        }
    }
}
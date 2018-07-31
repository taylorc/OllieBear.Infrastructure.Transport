using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Transport.RabbitMQ.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRabbitMqServices(this IServiceCollection services)
        {
            services.AddTransient<IChannelFactory, ChannelFactory>();

            return services;
        }
    }
}
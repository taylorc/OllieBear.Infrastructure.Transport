using System;
using Infrastructure.Transport.Interfaces;
using Infrastructure.Transport.Interfaces.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Transport.AzureServiceBus.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAzureServiceBusService(this IServiceCollection services)
        {
            services.AddTransient<IChannelFactory, ChannelFactory>();

            services.AddSingleton<ITopology, Topology>();

            services.AddTransient<Func<QueueConfigurationOptions, IConsumer>>(
                s =>
                    q => ActivatorUtilities.CreateInstance<Consumer>(s, q));

            services.AddTransient<Func<QueueConfigurationOptions, IProducer>>(
                s =>
                    q => ActivatorUtilities.CreateInstance<Producer>(s, q));

            return services;
        }
    }
}
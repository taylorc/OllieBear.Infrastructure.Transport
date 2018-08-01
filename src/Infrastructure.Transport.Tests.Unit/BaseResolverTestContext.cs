using System;
using System.IO;
using System.Linq;
using Infrastructure.Transport.Interfaces;
using Infrastructure.Transport.Interfaces.Options;
using Infrastructure.Transport.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Infrastructure.Transport.Tests.Unit
{
    internal class BaseResolverTestContext
    {
        protected readonly ServiceCollection Services;
        protected IConfigurationRoot Configuration;
        protected IServiceProvider ServiceProvider;
        protected ITopology Topology;
        protected TransportConfigurationOptions TransportConfigurationOptions;

        public BaseResolverTestContext()
        {
            Services = new ServiceCollection();
        }

        public void ArrangeContainerConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Services
                .Configure<TransportConfigurationOptions>(Configuration.GetSection("TransportConfigurationOptions"));
        }

        public void ArrangeContainerConfigurationNoProducers()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.only-consumers.json")
                .Build();

            Services
                .Configure<TransportConfigurationOptions>(Configuration.GetSection("TransportConfigurationOptions"));
        }

        public void AssertResolved()
        {
            Topology = ServiceProvider.GetService<ITopology>();
            TransportConfigurationOptions = Configuration.Get<TransportConfigurationOptions>();

            Assert.NotNull(Topology);
            Assert.NotNull(TransportConfigurationOptions);

            Assert.Equal(2, Topology.GetConsumers().Count());
        }

        protected class MessageHandler : IMessageHandler
        {
            public void Handle(object msg, Type type) { }

            public void HandleException(Exception exception) { }
        }
    }
}

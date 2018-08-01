using System;
using System.IO;
using System.Linq;
using Infrastructure.Transport.Interfaces;
using Infrastructure.Transport.Interfaces.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Infrastructure.Transport.Tests.Unit
{
    internal class BaseResolverTestContext
    {
        protected readonly ServiceCollection Services;
        protected readonly IConfigurationRoot Configuration;
        protected IServiceProvider ServiceProvider;
        protected ITopology Topology;
        protected TransportConfigurationOptions TransportConfigurationOptions;

        public BaseResolverTestContext()
        {
            Services = new ServiceCollection();

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public void ArrangeContainerConfiguration()
        {
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
            Assert.Equal(2, Topology.GetProducers().Count());
        }

        protected class MessageHandler : IMessageHandler
        {
            public void Handle(object msg, Type type) { }

            public void HandleException(Exception exception) { }
        }
    }
}

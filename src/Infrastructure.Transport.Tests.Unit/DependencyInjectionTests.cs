using System.Linq;
using Infrastructure.Logging.Serilog.DependencyInjection;
using Infrastructure.Serialization;
using Infrastructure.Serialization.Interfaces;
using Infrastructure.Transport.Interfaces;
using Infrastructure.Transport.RabbitMQ;
using Infrastructure.Transport.RabbitMQ.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Infrastructure.Transport.Tests.Unit
{
    public class DependencyInjectionTests
    {
        private readonly TestContext _context;

        public DependencyInjectionTests()
        {
            _context = new TestContext();
        }

        [Fact]
        public void Can_resolve_default_interfaces()
        {
            _context.ArrangeContainerConfiguration();
            _context.ActInjectDependencies();
            _context.AssertResolved();
        }

        [Fact]
        public void Can_resolve_default_interfaces_sans_configuration_section()
        {
            _context.ArrangeContainerConfigurationNoProducers();
            _context.ActInjectDependencies();
            _context.AssertResolved();
        }

        private class TestContext : BaseResolverTestContext
        {
            public void ActInjectDependencies()
            {
                Services
                    .AddTransient<ISerializer, SerializerJson>();

                Services
                    .AddSingleton<IMessageHandler, MessageHandler>();

                Services.AddSerilogLogging();

                Services.AddRabbitMqService();

                ReplaceChannelFactory(Services);

                ServiceProvider = Services.BuildServiceProvider();
            }

            private static void ReplaceChannelFactory(ServiceCollection services)
            {
                var descriptor = services.First(s => s.ServiceType == typeof(IChannelFactory));

                services.Remove(descriptor);

                services.AddTransient<IChannelFactory, EmptyChannelFactory>();
            }
        }
    }
}

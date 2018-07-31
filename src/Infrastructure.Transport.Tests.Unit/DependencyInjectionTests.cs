using Infrastructure.Logging.Serilog.DependencyInjection;
using Infrastructure.Serialization;
using Infrastructure.Serialization.Interfaces;
using Infrastructure.Transport.Interfaces;
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

                ServiceProvider = Services.BuildServiceProvider();
            }
        }
    }
}

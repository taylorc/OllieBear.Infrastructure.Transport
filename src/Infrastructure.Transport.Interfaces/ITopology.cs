using System.Collections.Generic;

namespace Infrastructure.Transport.Interfaces
{
    public interface ITopology
    {
        IConsumer GetConsumer(string key);

        IProducer GetProducer(string key);

        IEnumerable<IConsumer> GetConsumers();

        IEnumerable<IProducer> GetProducers();
    }
}

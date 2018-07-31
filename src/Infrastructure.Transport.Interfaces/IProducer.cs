using System;

namespace Infrastructure.Transport.Interfaces
{
    public interface IProducer : IDisposable
    {
        string Publish<T>(T msg);
    }
}

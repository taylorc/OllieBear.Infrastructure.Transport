using System;
using System.Threading.Tasks;

namespace Infrastructure.Transport.Interfaces
{
    public interface IConsumer : IDisposable
    {
        Task StartConsuming();
    }
}

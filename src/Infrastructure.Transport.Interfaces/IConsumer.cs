using System.Threading.Tasks;

namespace Infrastructure.Transport.Interfaces
{
    public interface IConsumer
    {
        Task StartConsuming();
    }
}

namespace Infrastructure.Transport.Interfaces
{
    public interface IProducer
    {
        string Publish<T>(T msg);
    }
}

using System;

namespace Infrastructure.Transport.Interfaces
{
    public interface IMessageHandler
    {
        void Handle(object msg, Type type);

        void HandleException(Exception exception);
    }
}

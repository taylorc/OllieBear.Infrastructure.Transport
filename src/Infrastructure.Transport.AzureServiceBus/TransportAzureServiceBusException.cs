using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Transport.AzureServiceBus
{
    public class TransportAzureServiceBusException:Exception
    {
        public TransportAzureServiceBusException(string message): base(message)
        {
                
        }

        public TransportAzureServiceBusException(string message, Exception exception):base(message,exception)
        {
                
        }
    }
}

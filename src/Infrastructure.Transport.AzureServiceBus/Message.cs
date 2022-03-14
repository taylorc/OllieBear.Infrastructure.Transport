using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Transport.AzureServiceBus
{
    public class Message
    {
        public string ContentType { get; set; }
        public string Body { get; set; }
    }
}

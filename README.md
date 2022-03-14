# vprc-infrastructure-transport

Standardised transport infrastructure module for VPRC projects

## Implementations
### DependencyInjection
#### RabbitMQ
##### Prerequisites
This module requires implementations of the following interfaces:
* ILog (Infrastructure.Logging)
* ISerializer (Infrastructure.Serialization)

To load the RabbitMQ module via DependencyInjection, add the following to the ServicesCollection registration:
```C#
var services = new ServiceCollection();

...

Services.AddRabbitMqService();
```

## Topology Configuration Options
You will define a series of keyed *consumer* and *producer* queues in the configuration, as well as the overall MQ connectivity
### Sample appsettings.json
```json

  "TransportConfigurationOptions": {
    "HostName": "mqhost.com",
    "MaxMessageSize": 512000,
    "VirtualHostName": "/",
    "UserName": "user",
    "Password": "pass",
    "ConsumerQueues": [
      {
        "Key": "A_Unique_Key_1",
        "Options": {
          "QueueName": "queue.qa.messaging",
          "Durable": true
        }
      },
      {
        "Key": "A_Unique_Key_2",
        "Options": {
          "QueueName": "queue.qa.reports",
          "Durable": true
        }
      }
    ],
    "ProducerQueues": [
      {
        "Key": "A_Unique_Key_1",
        "Options": {
          "QueueName": "queue.qa.messaging",
          "Durable": true
        }
      },
      {
        "Key": "A_Unique_Key_2",
        "Options": {
          "QueueName": "queue.qa.reports",
          "Durable": true
        }
      }
    ]
  }
```
>For Azure Service Bus only the TransportConfigurationOptions --> HostName, Producers and Consumers need to be populated. The HostName is the ConnectionString.

----------

### Resolving the consumer / producer
Use the defined keys to retrieve the applicable channel i.e.
```C#
private readonly ITopology _topology;

public Service(ITopology topology)
{
	_topology = topology;
}

...

IProducer producer = _topology.GetProducer("A_Unique_Key_2");
```
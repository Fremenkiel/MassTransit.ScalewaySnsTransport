# MassTransit ScalewaySnsTransport
MassTransit transporter for Scaleway SNS/SQS service based on MassTransit Amazon SQS transporter.

## Usage
The transporter is based on the official MassTransit Amazon SQS transporter, as the Scaleway Topics & Events/Queues products it self is a fork of the Amazon SNS and SQS platforms. 

For at complete guide on usage, please refer to the official MassTransit docs: https://masstransit.io/documentation/configuration/transports/amazon-sqs

## Minimal Example
The only major difference in usage is the setup process, as Scaleway uses seperat credentials for their Topics & Events and their Queues services.

In the example below, the Scaleway SNS and SQS settings are configured.

```
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.UsingScalewaySns((context, cfg) =>
                    {
                        cfg.Host("fr-par", h =>
                        {
                            h.Scope("project-XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"); // Your project id prefixed with 'project-'

                            h.SnsAccessKey("your-topics-access-key");
                            h.SnsSecretKey("your-topics-secret-key");

                            h.SqsAccessKey("your-queues-access-key");
                            h.SqsSecretKey("your-queues-secret-key");

                        });
                    });
                });
            })
            .Build()
            .RunAsync();
    }
}
```

## Base source code
For the original Amazon SQS transporter, please refer to the MassTransit source code: https://github.com/MassTransit/MassTransit
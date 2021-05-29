using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using MassTransit;

namespace Consumer
{
    class Program
    {
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("event-listener", e =>
                {
                    e.Consumer<EventReceiveData>();
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                Console.WriteLine("Press enter to exit");

                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
        
        public class EventReceiveData : IConsumer<CustomerRegistered>
        {
            public Task Consume(ConsumeContext<CustomerRegistered> context)
            {
                Console.WriteLine($"Received [x] {context.Message.Name} [at] {DateTime.Now:u}");
                return Task.CompletedTask;
            }
        }
    }
}

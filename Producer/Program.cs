using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Contracts;
using System.Threading;

namespace Producer
{
    class Program
    {
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                sbc.Publish<CustomerRegistered>(x =>
                {
                    x.Durable = true; // default: true
                    x.AutoDelete = false; // default: false
                    x.ExchangeType = "fanout"; // default, allows any valid exchange type
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                while (true)
                {
                    string value = await Task.Run(() =>
                    {
                        Console.WriteLine("Enter message (or quit to exit)");
                        Console.Write("> ");
                        return Console.ReadLine();
                    });

                    if("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                        break;

                    await busControl.Publish<CustomerRegistered>(new
                    {
                        Id = Guid.NewGuid(),
                        RegisterUtc = DateTime.Now,
                        Name = value
                    });
                }
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}

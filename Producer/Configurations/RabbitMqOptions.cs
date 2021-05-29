using System;

namespace Producer.Configurations
{
    public class RabbitMqOptions
    {
        public const string Url = "rabbitmq://localhost";
        public const string UserName = "guest";
        public const string Password = "guest";
        public const string QueueName = "notification";
    }
}

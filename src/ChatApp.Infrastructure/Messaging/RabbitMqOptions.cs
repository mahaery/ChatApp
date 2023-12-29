namespace ChatApp.Infrastructure.Messaging;
public class RabbitMqOptions
{
    public string HostName { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
    public string VirtualHost { get; init; }
    public string QueueName { get; init; }
    public string ExchangeName { get; init; }
    public string RoutingKey { get; init; }
}

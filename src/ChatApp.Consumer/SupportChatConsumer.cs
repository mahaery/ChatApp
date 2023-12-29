using ChatApp.Application.Common.Interfaces.Messaging;
using ChatApp.Domain.Entities;
using ChatApp.Infrastructure.Messaging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ChatApp.Consumer;
public class SupportChatConsumer
{
    private readonly IChatSessionQueueService _chatSessionQueueService;
    private readonly RabbitMqOptions _rabbitMqOptions;

    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;

    public SupportChatConsumer(
        IOptions<RabbitMqOptions> options,
        IChatSessionQueueService chatSessionQueueService)
    {
        _chatSessionQueueService = chatSessionQueueService;
        _rabbitMqOptions = options.Value;

        var factory = new ConnectionFactory
        {
            //Uri = new Uri(RabbitMQConnectionString)
            Uri = new($"amqp://{_rabbitMqOptions.UserName}:{_rabbitMqOptions.Password}@{_rabbitMqOptions.HostName}:5672/")
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _consumer = new EventingBasicConsumer(_channel);
        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
    }

    public void StartConsuming()
    {
        try
        {
            _channel.QueueDeclare(
                queue: _rabbitMqOptions.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            _channel.ExchangeDeclare(
                exchange: _rabbitMqOptions.ExchangeName,
                type: ExchangeType.Direct);

            _channel.QueueBind(
                queue: _rabbitMqOptions.QueueName, 
                exchange: _rabbitMqOptions.ExchangeName, 
                routingKey: _rabbitMqOptions.RoutingKey);

            _consumer.Received += async (model, eventArgs) =>
            {
                Thread.Sleep(1000);
                var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                // Process the chat message and assign it to an agent
                if (string.IsNullOrEmpty(message)) return;

                // Deserialize the json to the chat object
                var chatSession = JsonConvert.DeserializeObject<ChatSession>(message);

                if (chatSession == null) return;

                var result = await _chatSessionQueueService.AssignChatToAgentAsync(chatSession);
                if (result) _channel.BasicAck(eventArgs.DeliveryTag, false);
            };
            _channel.BasicConsume(queue: _rabbitMqOptions.QueueName, autoAck: false, consumer: _consumer);

        }
        catch (Exception e)
        {
            // Handle exceptions and log errors
        }
    }

    public void StopConsuming()
    {
        _channel.Close();
        _connection.Close();

        Console.WriteLine("Support chat consumer stopped.");
    }
}

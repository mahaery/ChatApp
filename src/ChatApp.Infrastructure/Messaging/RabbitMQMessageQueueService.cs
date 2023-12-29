using ChatApp.Application.Common.Interfaces.Messaging;
using ChatApp.Application.Common.Interfaces.Persistence;
using ChatApp.Infrastructure.Helpers;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace ChatApp.Infrastructure.Messaging;
public class RabbitMQMessageQueueService : IMessageQueueService
{
    private readonly RabbitMqOptions _rabbitMqOptions;
    private readonly IConnectionFactory _connectionFactory;
    private readonly ITeamRepository _teamRepository;

    public RabbitMQMessageQueueService(
        ITeamRepository teamRepository,
        IOptions<RabbitMqOptions> options)
    {
        _teamRepository = teamRepository;
        _rabbitMqOptions = options.Value;

        _connectionFactory = new ConnectionFactory
        {
            HostName = _rabbitMqOptions.HostName,
            UserName = _rabbitMqOptions.UserName,
            Password = _rabbitMqOptions.Password,
            VirtualHost = _rabbitMqOptions.VirtualHost,
        };

        using (var connection = _connectionFactory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(
                exchange: "Chat",
                type: ExchangeType.Direct);

            channel.QueueDeclare(
                _rabbitMqOptions.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            channel.QueueBind(
                queue: _rabbitMqOptions.QueueName,
                exchange: _rabbitMqOptions.ExchangeName,
                routingKey: _rabbitMqOptions.RoutingKey);
        }
    }

    public async Task<bool> EnqueueMessage(string message)
    {
        try
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: _rabbitMqOptions.ExchangeName, 
                    routingKey: _rabbitMqOptions.RoutingKey, 
                    basicProperties: null, 
                    body: body);
            }

            return await Task.FromResult(true);
        }
        catch
        {
            // Handle exceptions and log errors
        }
        return await Task.FromResult(false);
    }
    public async Task<int> GetMessageCount()
    {
        try
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queueDeclareOk = channel.QueueDeclarePassive(_rabbitMqOptions.QueueName);
                return await Task.FromResult((int)queueDeclareOk.MessageCount);
            }
        }
        catch
        {
            // Handle exceptions and log errors
        }

        return int.MaxValue;
    }
    public async Task<bool> IsSessionQueueFull()
    {
        try
        {
            var messageCount = await GetMessageCount();
            var activeTeam = await _teamRepository.GetActiveTeam();
            var queueSize = (int)(activeTeam.GetTeamCapacity() * 1.5);

            var isOverflowTeamAvailable = await _teamRepository.IsOverflowTeamAvailable();
            var isDuringOfficeHour = TimeHelper.IsDuringOfficeHours();
            if (isOverflowTeamAvailable && isDuringOfficeHour)
            {
                var overflowTeam = await _teamRepository.GetOverflowTeam();
                var overflowTeamSize = (int)(overflowTeam.GetTeamCapacity() * 1.5);
                queueSize += overflowTeamSize;
            }

            var isFull = messageCount >= queueSize;
            return isFull;
        }
        catch
        {
            // Handle exceptions and log errors
        }

        return true;
    }
}
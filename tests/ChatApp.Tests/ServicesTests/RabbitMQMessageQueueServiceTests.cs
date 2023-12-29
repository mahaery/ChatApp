using ChatApp.Application.Common.Interfaces.Persistence;
using ChatApp.Infrastructure.Messaging;
using Microsoft.Extensions.Options;

namespace ChatApp.Tests.ServicesTests;
public class RabbitMqMessageQueueServiceTests
{
    [Fact]
    public async Task EnqueueMessage_SuccessfullyEnqueuesMessage()
    {
        // Arrange
        var teamRepositoryMock = new Mock<ITeamRepository>();
        var options = Options.Create(new RabbitMqOptions
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/",
            ExchangeName = "Chat",
            QueueName = "ChatQueue",
            RoutingKey = "Chat"
        });
        var service = new Mock<RabbitMQMessageQueueService>(teamRepositoryMock.Object, options);
        var message = "Test message";

        // Act
        var result = await service.Object.EnqueueMessage(message);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetMessageCount_ReturnsMessageCount()
    {
        // Arrange
        var teamRepositoryMock = new Mock<ITeamRepository>();
        var options = Options.Create(new RabbitMqOptions
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/",
            ExchangeName = "Chat",
            QueueName = "ChatQueue",
            RoutingKey = "Chat"
        });
        var service = new RabbitMQMessageQueueService(teamRepositoryMock.Object, options);

        // Act
        var result = await service.GetMessageCount();

        // Assert
        result.Should().BeOfType(typeof(int));
        result.Should().BeGreaterOrEqualTo(0);
    }
}

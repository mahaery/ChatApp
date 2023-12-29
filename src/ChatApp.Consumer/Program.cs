using ChatApp.Api.OptionsSetup;
using ChatApp.Application;
using ChatApp.Application.Common.Interfaces.Messaging;
using ChatApp.Infrastructure;
using ChatApp.Infrastructure.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ChatApp.Consumer;
internal class Program
{
    static void Main()
    {
        var serviceProvider = ConfigureServices();

        var supportChatConsumer = new SupportChatConsumer(
            options: serviceProvider.GetService<IOptions<RabbitMqOptions>>(),
            chatSessionQueueService: serviceProvider.GetService<IChatSessionQueueService>());

        supportChatConsumer.StartConsuming();

        Console.ReadLine();
        supportChatConsumer.StopConsuming();
    }
    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.Development.json")
            .Build();

        services.AddOptions<RabbitMqOptions>()
            .Bind(configuration.GetSection(RabbitMqOptionsSetup.SectionName));

        // Register your dependencies
        services.AddApplication()
            .AddPersistence();

        return services.BuildServiceProvider();
    }
}
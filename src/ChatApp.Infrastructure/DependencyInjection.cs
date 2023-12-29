using ChatApp.Application.Common.Interfaces.Messaging;
using ChatApp.Application.Common.Interfaces.Persistence;
using ChatApp.Infrastructure.Messaging;
using ChatApp.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence();

        return services;
    }
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IAgentRepository, AgentRepository>();
        services.AddScoped<IChatSessionRepository, ChatSessionRepository>();
        services.AddTransient<ITeamRepository, TeamRepository>();
        services.AddTransient<IMessageQueueService, RabbitMQMessageQueueService>();
        services.AddScoped<IChatSessionQueueService, ChatSessionQueueService>();

        return services;
    }
}
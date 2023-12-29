using ChatApp.Infrastructure.Messaging;
using Microsoft.Extensions.Options;

namespace ChatApp.Api.OptionsSetup;
public sealed class RabbitMqOptionsSetup : IConfigureOptions<RabbitMqOptions>
{
    public const string SectionName = "RabbitMq:Connection";
    private readonly IConfiguration _configuration;

    public RabbitMqOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void Configure(RabbitMqOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}

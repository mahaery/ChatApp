using ChatApp.Domain.Entities;

namespace ChatApp.Application.Common.Interfaces.Messaging;
public interface IChatSessionQueueService
{
    Task<Guid> CreateChatSession(Guid userId, bool isActive);
    Task<bool> AssignChatToAgentAsync(ChatSession chatSession);
}
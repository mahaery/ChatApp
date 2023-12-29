using ChatApp.Domain.Entities;

namespace ChatApp.Application.Common.Interfaces.Persistence;
public interface IChatSessionRepository
{
    Task<ChatSession> GetChatSessionById(Guid chatSessionId);
    Task<IEnumerable<ChatSession>> GetActiveChatSessions();
    Task<bool> AddChatSession(ChatSession chatSession);
    Task<bool> UpdateChatSessionStatus(ChatSession chatSession);
}
using ChatApp.Application.Common.Interfaces.Persistence;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Persistence;
public class ChatSessionRepository : IChatSessionRepository
{
    private readonly List<ChatSession> _chatSessions;
    public ChatSessionRepository()
    {
        _chatSessions = new List<ChatSession>();
    }

    public async Task<ChatSession> GetChatSessionById(Guid chatSessionId)
    {
        var chatSession = _chatSessions.FirstOrDefault(chatSession => chatSession.Id == chatSessionId);
        return await Task.FromResult(chatSession);
    }

    public async Task<IEnumerable<ChatSession>> GetActiveChatSessions()
    {
        var activeChatSessions = _chatSessions.Where(session => session.IsActive);
        return await Task.FromResult(activeChatSessions);
    }

    public async Task<bool> AddChatSession(ChatSession chatSession)
    {
        _chatSessions.Add(chatSession);
        return await Task.FromResult(true);
    }

    public async Task<bool> UpdateChatSessionStatus(ChatSession chatSession)
    {
        var existingSession = _chatSessions.FirstOrDefault(session => session.Id == chatSession.Id);
        if (existingSession == null)
            return await Task.FromResult(false);

        existingSession.UpdateDetails(chatSession);

        return await Task.FromResult(true);
    }
}

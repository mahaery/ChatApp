using ChatApp.Domain.Common;
using Newtonsoft.Json;

namespace ChatApp.Domain.Entities;
public class ChatSession : BaseEntity
{
    public Guid UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }
    public int PollCount { get; private set; }
    [JsonConstructor]
    protected ChatSession(Guid userId, bool isActive)
    {
        UserId = userId;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
    }
    public static ChatSession Create(Guid userId, bool isActive)
        => new ChatSession(userId, isActive);

    public void MarkInactive() => IsActive = false;

    public void IncrementPollCount()
    {
        PollCount++;
    }

    public void UpdateDetails(ChatSession chatSession)
    {
        IsActive = chatSession.IsActive;
        PollCount = chatSession.PollCount;
    }
}
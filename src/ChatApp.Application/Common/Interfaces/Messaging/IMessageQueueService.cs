namespace ChatApp.Application.Common.Interfaces.Messaging;
public interface IMessageQueueService
{
    Task<bool> EnqueueMessage(string message);
    Task<int> GetMessageCount();
    Task<bool> IsSessionQueueFull();
}
using ChatApp.Application.Common.Interfaces.Messaging;
using ChatApp.Domain.Entities;
using System.Text.Json;
using ChatApp.Application.Common.Interfaces.Persistence;
using ChatApp.Infrastructure.Helpers;

namespace ChatApp.Infrastructure.Messaging;
public class ChatSessionQueueService : IChatSessionQueueService
{
    private readonly IMessageQueueService _messageQueueService;
    private readonly ITeamRepository _teamRepository;

    public ChatSessionQueueService(
        IMessageQueueService messageQueueService,
        ITeamRepository teamRepository)
    {
        _messageQueueService = messageQueueService;
        _teamRepository = teamRepository;
    }

    public async Task<Guid> CreateChatSession(Guid userId, bool isActive)
    {
        var isQueueFull = await _messageQueueService.IsSessionQueueFull();

        if (isQueueFull)
        {
            // Refuse chat if the session queue is full and it's not during office hours
            throw new Exception("There are no available agents at this time. Please try again later.");
        }
        // Create a new chat session
        var chatSession = ChatSession.Create(userId, true);

        // Enqueue the chat session
        var jsonBody = JsonSerializer.Serialize(chatSession);
        var messageQueued = await _messageQueueService.EnqueueMessage(jsonBody);

        if (messageQueued is false)
            throw new Exception("Something went wrong during publishing your message. Please try again later.");

        // Return the session ID
        return chatSession.Id;
    }

    public async Task<bool> AssignChatToAgentAsync(ChatSession chatSession)
    {
        try
        {
            // Get the active team from repository
            var team = await _teamRepository.GetActiveTeam();

            Console.WriteLine($"Team Capacity {team.GetTeamCapacity()} and Team Tasks are {team.Members.Select(x => x.AssignedChatSessions.Count).Sum()}");

            // Get the available Agent 
            // Chats are assigned in a round robin fashion, preferring to assign the junior first, then mid, then senior etc.
            // This ensures that the higher seniority are more available to assist the lower
            var agent = team.Members
                .OrderBy(x => x.Seniority)
                .ThenBy(x => x.AssignedChatSessions.Count)
                .FirstOrDefault(m => m.IsAvailable() && m.IsAssignable());

            if (agent is not null)
            {
                agent.AssignChatSessionToAgent(chatSession);
                Console.WriteLine($"Message assigned to agent {agent.Name} in {team.Name}");
                return true;
            }

            // Check if during office hours and overflow team is available and throw if not
            if (!TimeHelper.IsDuringOfficeHours() || !await _teamRepository.IsOverflowTeamAvailable())
                throw new Exception("No Agent Available at the moment");

            var overflowTeam = await _teamRepository.GetOverflowTeam();

            Console.WriteLine($"Overflow Team Capacity {overflowTeam.GetTeamCapacity()} and Team Tasks are {overflowTeam.Members.Select(x => x.AssignedChatSessions.Count).Sum()}");

            var overflowAgent = overflowTeam.Members
                .OrderBy(x => x.Seniority)
                .ThenBy(x => x.AssignedChatSessions.Count)
                .FirstOrDefault(m => m.IsAvailable() && m.IsAssignable());

            if (overflowAgent is null)
            {
                throw new Exception("No Agent Available at the moment");
            }

            overflowAgent.AssignChatSessionToAgent(chatSession);

            Console.WriteLine($"Message assigned to agent {overflowAgent.Name} in {overflowTeam.Name}");

            return true;
        }
        catch (Exception e)
        {
            // Handle exceptions and log errors
        }
        return false;
    }
}

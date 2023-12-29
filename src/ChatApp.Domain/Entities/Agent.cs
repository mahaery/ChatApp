using ChatApp.Domain.Common;
using ChatApp.Domain.Enums;
using ChatApp.Domain.ValueObjects;

namespace ChatApp.Domain.Entities;
public class Agent : BaseEntity
{
    private readonly List<ChatSession> _assignedChatSessions = new();

    public string Name { get; private set; }
    public SeniorityLevel Seniority { get; private set; }
    public Shift CurrentShift { get; private set; }
    public int Capacity { get; }
    public IReadOnlyList<ChatSession> AssignedChatSessions => _assignedChatSessions.AsReadOnly();

    protected Agent(string name,
        SeniorityLevel seniority,
        int shiftNumber,
        TimeOnly startTime,
        TimeOnly endTime)
    {
        Name = name;
        Seniority = seniority;
        CurrentShift = new Shift(
            shiftNumber,
            startTime,
            endTime);

        Capacity = (int)(10 * GetEfficiency());
    }

    public static Agent Create(
        string name,
        SeniorityLevel seniority,
        int shiftNumber,
        TimeOnly startTime,
        TimeOnly endTime)
            => new Agent(
                name,
                seniority,
                shiftNumber,
                startTime,
                endTime);

    public bool IsAvailable()
    {
        return (
            TimeOnly.FromDateTime(DateTime.UtcNow) >= CurrentShift.StartTime && 
            TimeOnly.FromDateTime(DateTime.UtcNow) <= CurrentShift.EndTime);
    }

    public bool IsAssignable()
    {
        var isAssignedQueueFull = (Capacity - AssignedChatSessions.Count) == 0;

        return !isAssignedQueueFull;
    }

    public double GetEfficiency() =>
        Seniority switch
        {
            SeniorityLevel.Junior => 0.4,
            SeniorityLevel.MidLevel => 0.6,
            SeniorityLevel.Senior => 0.8,
            SeniorityLevel.TeamLead => 0.5,
            _ => 1.0,
        };

    public void AssignChatSessionToAgent(ChatSession chatSession)
    {
        _assignedChatSessions.Add(chatSession);
    }
}
using ChatApp.Domain.Common;
using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Entities;
public class Team : BaseEntity
{
    private readonly List<Agent> _agents = new();

    public string Name { get; private set; }
    public IReadOnlyList<Agent> Members => _agents.AsReadOnly();
    public TeamType Type { get; private set; }

    protected Team(
        string name,
        TeamType type,
        params Agent[] agents)
    {
        Name = name;
        _agents.AddRange(agents);
        Type = type;
    }


    public static Team Create(
        string name,
        TeamType type,
        params Agent[] agents)
        => new(
            name,
            type,
            agents);

    public void UpdateDetails(string name)
    {
        Name = name;
    }

    public int GetTeamCapacity() => _agents.Sum(agent => agent.Capacity);
}
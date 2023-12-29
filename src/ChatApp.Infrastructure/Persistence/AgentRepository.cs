using ChatApp.Application.Common.Interfaces.Persistence;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Persistence;
public class AgentRepository : IAgentRepository
{
    private readonly List<Agent> _agents;

    public AgentRepository(IEnumerable<Team> teams)
    {
        _agents = teams.SelectMany(team => team.Members).ToList();
    }

    public async Task<IEnumerable<Agent>> GetAllAgents()
    {
        return await Task.FromResult(_agents);
    }

    public async Task<IEnumerable<Agent>> GetAvailableAgents()
    {
        return await Task.FromResult(_agents.Where(agent => agent.IsAvailable()));
    }

    public async Task<bool> UpdateAgent(Agent agent)
    {
        throw new NotImplementedException();
    }
}
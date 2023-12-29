using ChatApp.Domain.Entities;

namespace ChatApp.Application.Common.Interfaces.Persistence;
public interface IAgentRepository
{
    Task<IEnumerable<Agent>> GetAllAgents();
    Task<IEnumerable<Agent>> GetAvailableAgents();
    Task<bool> UpdateAgent(Agent agent);
}
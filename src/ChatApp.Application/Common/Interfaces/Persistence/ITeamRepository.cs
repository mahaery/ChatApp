using ChatApp.Domain.Entities;

namespace ChatApp.Application.Common.Interfaces.Persistence;
public interface ITeamRepository
{
    Task<IEnumerable<Team>> GetAllTeams();
    Task<Team> GetTeamById(Guid teamId);
    Task<Team> GetActiveTeam();
    Task<Team> GetOverflowTeam();
    Task<bool> AddTeam(Team team);
    Task<bool> IsOverflowTeamAvailable();
    Task<bool> UpdateTeam(Team team);
}
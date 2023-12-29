using ChatApp.Application.Common.Interfaces.Persistence;
using ChatApp.Domain;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;

namespace ChatApp.Infrastructure.Persistence;
public class TeamRepository : ITeamRepository
{
    private readonly List<Team> _teams;

    public TeamRepository()
    {
        _teams = new List<Team>
        {
            SeedData.AddTeamA(),
            SeedData.AddTeamB(),
            SeedData.AddTeamC(),
            SeedData.AddOverflowTeam()
        };
    }
    public async Task<IEnumerable<Team>> GetAllTeams()
    {
        return await Task.FromResult(_teams);
    }

    public async Task<Team> GetTeamById(Guid teamId)
    {
        var team = _teams.FirstOrDefault(t => t.Id == teamId);
        return await Task.FromResult(team);
    }

    public async Task<Team> GetActiveTeam()
    {
        var team = _teams.First(t => t.Members.All(x => x.CurrentShift.IsActive()));
        return await Task.FromResult(team);
    }

    public async Task<Team> GetOverflowTeam()
    {
        var team = _teams.FirstOrDefault(t => t.Type == TeamType.Overflow);
        return await Task.FromResult(team);
    }

    public async Task<bool> AddTeam(Team team)
    {
        _teams.Add(team);
        return await Task.FromResult(true);
    }

    public async Task<bool> IsOverflowTeamAvailable()
    {
        var team = _teams.FirstOrDefault(t => t.Type == TeamType.Overflow);
        if (team is null)
            return await Task.FromResult(false);

        return await Task.FromResult(true);
    }

    public async Task<bool> UpdateTeam(Team team)
    {
        var existingTeam = _teams.FirstOrDefault(t => t.Id == team.Id);
        if (existingTeam == null)
            return await Task.FromResult(false);

        existingTeam.UpdateDetails(team.Name);

        return await Task.FromResult(true);
    }
}

using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;

namespace ChatApp.Tests.EntitiesTests;
public class TeamTests
{
    [Fact]
    public void Create_ValidArguments_CreatesTeamWithAgents()
    {
        // Arrange
        var teamName = "Team A";
        var teamType = TeamType.Normal;
        var agent1 = Agent.Create("Agent 1", SeniorityLevel.Junior, 1, new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0));
        var agent2 = Agent.Create("Agent 2", SeniorityLevel.MidLevel, 1, new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0));
        var agent3 = Agent.Create("Agent 3", SeniorityLevel.MidLevel, 1, new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0));

        // Act
        var team = Team.Create(teamName, teamType, agent1, agent2, agent3);

        // Assert
        team.Should().NotBeNull();
        team.Name.Should().Be(teamName);
        team.Type.Should().Be(teamType);
        team.Members.Should().HaveCount(3);
        team.Members.Should().Contain(agent1);
        team.Members.Should().Contain(agent2);
        team.Members.Should().Contain(agent3);
    }

    [Fact]
    public void UpdateDetails_ValidName_UpdatesTeamName()
    {
        // Arrange
        var teamName = "Team A";
        var updatedTeamName = "Updated Team A";
        var team = Team.Create(teamName, TeamType.Normal);

        // Act
        team.UpdateDetails(updatedTeamName);

        // Assert
        team.Name.Should().Be(updatedTeamName);
    }

    [Fact]
    public void GetTeamCapacity_ReturnsSumOfAgentCapacities()
    {
        // Arrange
        var teamName = "Team A";
        var teamType = TeamType.Normal;
        var agent1 = Agent.Create("Agent 1", SeniorityLevel.Junior, 1, new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0));
        var agent2 = Agent.Create("Agent 2", SeniorityLevel.MidLevel, 1, new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0));
        var agent3 = Agent.Create("Agent 3", SeniorityLevel.MidLevel, 1, new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0));
        var team = Team.Create(teamName, teamType, agent1, agent2, agent3);

        // Act
        var teamCapacity = team.GetTeamCapacity();

        // Assert
        teamCapacity.Should().Be(agent1.Capacity + agent2.Capacity + agent3.Capacity);
    }
}

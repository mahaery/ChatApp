using ChatApp.Application.Common.Interfaces.Persistence;
using ChatApp.Domain;
using ChatApp.Domain.Entities;

namespace ChatApp.Tests.RepositoriesTests;
public class TeamRepositoryTests
{
    private readonly Mock<ITeamRepository> _teamRepositoryMock;

    public TeamRepositoryTests()
    {
        _teamRepositoryMock = new Mock<ITeamRepository>();
    }

    [Fact]
    public async Task GetAllTeams_ReturnsAllTeams()
    {
        // Arrange
        var teams = new List<Team>
        {
            SeedData.AddTeamA(),
            SeedData.AddTeamB(),
            SeedData.AddTeamC(),
            SeedData.AddOverflowTeam()
        };
        _teamRepositoryMock.Setup(repo => repo.GetAllTeams())
            .ReturnsAsync(teams);

        // Act
        var result = await _teamRepositoryMock.Object.GetAllTeams();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(teams.Count);
        result.Should().BeEquivalentTo(teams);
    }

    [Fact]
    public async Task GetTeamById_ValidId_ReturnsTeam()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var team = SeedData.AddTeamA(); // Use SeedData to create the team
        _teamRepositoryMock.Setup(repo => repo.GetTeamById(teamId))
            .ReturnsAsync(team);

        // Act
        var result = await _teamRepositoryMock.Object.GetTeamById(teamId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(team);
    }

    [Fact]
    public async Task GetOverflowTeam_ReturnsOverflowTeam()
    {
        // Arrange
        var overflowTeam = SeedData.AddOverflowTeam(); // Use SeedData to create the overflow team
        _teamRepositoryMock.Setup(repo => repo.GetOverflowTeam())
            .ReturnsAsync(overflowTeam);

        // Act
        var result = await _teamRepositoryMock.Object.GetOverflowTeam();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(overflowTeam);
    }

    [Fact]
    public async Task AddTeam_ReturnsTrue()
    {
        // Arrange
        var team = SeedData.AddTeamA(); // Use SeedData to create the team
        _teamRepositoryMock.Setup(repo => repo.AddTeam(It.IsAny<Team>()))
            .ReturnsAsync(true);

        // Act
        var result = await _teamRepositoryMock.Object.AddTeam(team);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsOverflowTeamAvailable_ReturnsTrue()
    {
        // Arrange
        var overflowTeam = SeedData.AddOverflowTeam(); // Use SeedData to create the overflow team
        _teamRepositoryMock.Setup(repo => repo.IsOverflowTeamAvailable())
            .ReturnsAsync(true);

        // Act
        var result = await _teamRepositoryMock.Object.IsOverflowTeamAvailable();

        // Assert
        result.Should().BeTrue();
    }
}

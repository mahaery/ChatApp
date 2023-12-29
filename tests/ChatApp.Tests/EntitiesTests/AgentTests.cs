using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;

namespace ChatApp.Tests.EntitiesTests;

public class AgentTests
{
    [Fact]
    public void Create_ValidArguments_CreatesAgent()
    {
        // Arrange
        var agentName = "Agent 1";
        var seniorityLevel = SeniorityLevel.Junior;
        var shiftNumber = 1;
        var startTime = new TimeOnly(8, 0, 0);
        var endTime = new TimeOnly(16, 0, 0);

        // Act
        var agent = Agent.Create(agentName, seniorityLevel, shiftNumber, startTime, endTime);

        // Assert
        agent.Should().NotBeNull();
        agent.Name.Should().Be(agentName);
        agent.Seniority.Should().Be(seniorityLevel);
        agent.CurrentShift.StartTime.Should().Be(startTime);
        agent.CurrentShift.EndTime.Should().Be(endTime);
        agent.Capacity.Should().Be((int)(10 * agent.GetEfficiency()));
    }
    
    [Fact]
    public void IsAssignable_AssignedQueueNotFull_ReturnsTrue()
    {
        // Arrange
        var agentName = "Agent 1";
        var seniorityLevel = SeniorityLevel.Junior;
        var shiftNumber = 1;
        var startTime = new TimeOnly(8, 0, 0);
        var endTime = new TimeOnly(16, 0, 0);
        var agent = Agent.Create(agentName, seniorityLevel, shiftNumber, startTime, endTime);

        // Assign a chat session to the agent
        var chatSession = ChatSession.Create(Guid.NewGuid(), true);
        agent.AssignChatSessionToAgent(chatSession);

        // Act
        var isAssignable = agent.IsAssignable();

        // Assert
        isAssignable.Should().BeTrue();
    }

    [Fact]
    public void IsAssignable_AssignedQueueFull_ReturnsFalse()
    {
        // Arrange
        var agentName = "Agent 1";
        var seniorityLevel = SeniorityLevel.Junior;
        var shiftNumber = 1;
        var startTime = new TimeOnly(8, 0, 0);
        var endTime = new TimeOnly(16, 0, 0);
        var agent = Agent.Create(agentName, seniorityLevel, shiftNumber, startTime, endTime);

        // Assign the maximum number of chat sessions to the agent
        for (var i = 0; i < agent.Capacity; i++)
        {
            var chatSession = ChatSession.Create(Guid.NewGuid(), true);
            agent.AssignChatSessionToAgent(chatSession);
        }

        // Act
        var isAssignable = agent.IsAssignable();

        // Assert
        isAssignable.Should().BeFalse();
    }

    [Fact]
    public void GetEfficiency_ReturnsCorrectEfficiencyBasedOnSeniorityLevel()
    {
        // Arrange
        var juniorAgent = Agent.Create("Junior Agent", SeniorityLevel.Junior, 1, new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0));
        var midLevelAgent = Agent.Create("Mid-Level Agent", SeniorityLevel.MidLevel, 1, new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0));
        var seniorAgent = Agent.Create("Senior Agent", SeniorityLevel.Senior, 1, new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0));
        var teamLeadAgent = Agent.Create("Team Lead Agent", SeniorityLevel.TeamLead, 1, new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0));

        // Act
        var juniorEfficiency = juniorAgent.GetEfficiency();
        var midLevelEfficiency = midLevelAgent.GetEfficiency();
        var seniorEfficiency = seniorAgent.GetEfficiency();
        var teamLeadEfficiency = teamLeadAgent.GetEfficiency();

        // Assert
        juniorEfficiency.Should().Be(0.4);
        midLevelEfficiency.Should().Be(0.6);
        seniorEfficiency.Should().Be(0.8);
        teamLeadEfficiency.Should().Be(0.5);
    }

    [Fact]
    public void AssignChatSessionToAgent_AddsChatSessionToAssignedChatSessions()
    {
        // Arrange
        var agentName = "Agent 1";
        var seniorityLevel = SeniorityLevel.Junior;
        var shiftNumber = 1;
        var startTime = new TimeOnly(8, 0, 0);
        var endTime = new TimeOnly(16, 0, 0);
        var agent = Agent.Create(agentName, seniorityLevel, shiftNumber, startTime, endTime);
        var chatSession = ChatSession.Create(Guid.NewGuid(), true);

        // Act
        agent.AssignChatSessionToAgent(chatSession);

        // Assert
        agent.AssignedChatSessions.Should().HaveCount(1);
        agent.AssignedChatSessions.Should().Contain(chatSession);
    }
}

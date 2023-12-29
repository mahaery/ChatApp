using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;

namespace ChatApp.Domain;
public static class SeedData
{
    public static Team AddTeamA()
    {
        var startTime = new TimeOnly(8, 0, 0);
        var endTime = new TimeOnly(16, 0, 0);

        return Team.Create(
            name: "Team A",
            type: TeamType.Normal,
            Agent.Create(
                name: "Team Lead 1",
                seniority: SeniorityLevel.TeamLead,
                shiftNumber: 1,
                startTime: startTime,
                endTime: endTime),
            Agent.Create(
                name: "Mid-Level 1",
                seniority: SeniorityLevel.MidLevel,
                shiftNumber: 1,
                startTime: startTime,
                endTime: endTime),
            Agent.Create(
                name: "Mid-Level 2",
                seniority: SeniorityLevel.MidLevel,
                shiftNumber: 1,
                startTime: startTime,
                endTime: endTime),
            Agent.Create(
                name: "Junior 1",
                seniority: SeniorityLevel.Junior,
                shiftNumber: 1,
                startTime: startTime,
                endTime: endTime));
    }
    public static Team AddTeamB()
    {
        var startTime = new TimeOnly(16, 0, 0);
        var endTime = new TimeOnly(23, 59, 59);

        return Team.Create(
            name: "Team B",
            type: TeamType.Normal,
            Agent.Create(
                name: "Senior 1",
                seniority: SeniorityLevel.Senior,
                shiftNumber: 2,
                startTime: startTime,
                endTime: endTime),
            Agent.Create(
                name: "Mid-Level 3",
                seniority: SeniorityLevel.MidLevel,
                shiftNumber: 2,
                startTime: startTime,
                endTime: endTime),
            Agent.Create(
                name: "Junior 2",
                seniority: SeniorityLevel.Junior,
                shiftNumber: 2,
                startTime: startTime,
                endTime: endTime),
            Agent.Create(
                name: "Junior 3",
                seniority: SeniorityLevel.Junior,
                shiftNumber: 2,
                startTime: startTime,
                endTime: endTime));
    }
    public static Team AddTeamC()
    {
        var startTime = new TimeOnly(0, 0, 01);
        var endTime = new TimeOnly(8, 0, 0);

        return Team.Create(
            name: "Team C",
            type: TeamType.Normal,
            Agent.Create(
                name: "Mid-Level 4",
                seniority: SeniorityLevel.MidLevel,
                shiftNumber: 3,
                startTime: startTime,
                endTime: endTime),
            Agent.Create(
                name: "Mid-Level 5",
                seniority: SeniorityLevel.MidLevel,
                shiftNumber: 3,
                startTime: startTime,
                endTime: endTime));
    }

    public static Team AddOverflowTeam()
    {
        var startTime = new TimeOnly(8,0,0);
        var endTime = new TimeOnly(16, 0, 0);

        var agents = new List<Agent>();
        for (int i = 4; i < 10; i++)
        {
            agents.Add(Agent.Create(
                name: $"Junior {i}",
                seniority: SeniorityLevel.Junior,
                shiftNumber: 2,
                startTime: startTime,
                endTime: endTime));
        }

        return Team.Create(
            name: "Overflow Team",
            type: TeamType.Overflow,
            agents.ToArray());
    }
}

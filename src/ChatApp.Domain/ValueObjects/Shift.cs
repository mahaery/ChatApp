namespace ChatApp.Domain.ValueObjects;

public record Shift(
    int ShiftNumber,
    TimeOnly StartTime,
    TimeOnly EndTime)
{
    public bool IsActive()
    {
        var condition = (TimeOnly.FromDateTime(DateTime.UtcNow) >= StartTime) && (TimeOnly.FromDateTime(DateTime.UtcNow) <= EndTime);
        return condition;
    }
}
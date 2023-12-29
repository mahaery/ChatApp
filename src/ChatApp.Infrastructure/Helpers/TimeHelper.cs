namespace ChatApp.Infrastructure.Helpers;
public static class TimeHelper
{
    public static bool IsDuringOfficeHours() =>
        (DateTime.UtcNow >= DateTime.Parse("08:00 AM") && DateTime.UtcNow <= DateTime.Parse("04:00 PM"));
}

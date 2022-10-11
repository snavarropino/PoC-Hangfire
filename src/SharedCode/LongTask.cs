using Hangfire;

namespace SharedCode;

public static class LongTask
{
    [Queue("long")]
    public static async Task Execute(int id, int duration)
    {
        Console.WriteLine($"Starting long task with Id={id} and Duration={duration}");
        await Task.Delay(TimeSpan.FromSeconds(duration));
        Console.WriteLine($"Long task with Id={id} and Duration={duration} is finished");
    }
}
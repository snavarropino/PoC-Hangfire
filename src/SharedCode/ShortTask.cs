using Hangfire;

namespace SharedCode;
public static class ShortTask
{
    [Queue("short")]
    public static async Task Execute(int id)
    {
        var duration = new Random().Next(5);
        Console.WriteLine($"Starting short task with Id={id} and Duration={duration}");
        await Task.Delay(TimeSpan.FromSeconds(duration));
        Console.WriteLine($"Short task with Id={id} and Duration={duration} is finished");
    }
}
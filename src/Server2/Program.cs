using Hangfire;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
    {
        //CommandBatchMaxTimeout = TimeSpan.FromMinutes(5), //Only works in .NET Frameworks
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer(options =>
{

    options.ServerName = $"{Environment.MachineName}-Server2-{Guid.NewGuid()}";
    // https://docs.hangfire.io/en/latest/background-processing/configuring-queues.html#asp-net-core
    options.Queues = new[] { "short", "default" };
    options.WorkerCount = Environment.ProcessorCount * 5; // This is the default value
});

Console.WriteLine("This is Server 2");

var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/", () => "This is Server 2");

app.Run();
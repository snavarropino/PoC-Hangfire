using Hangfire;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings());

var connectionString = builder.Configuration.GetConnectionString("HangfireConnection");
var sqlServerStorageOptions = new SqlServerStorageOptions
{
    PrepareSchemaIfNecessary = true,
    //CommandBatchMaxTimeout = TimeSpan.FromMinutes(5), //Only works in .NET Frameworks
    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5), //After this time the job will be visible again for reprocessing if havent finished (tiemout)
    QueuePollInterval = TimeSpan.Zero,
    UseRecommendedIsolationLevel = true,
    DisableGlobalLocks = true //Required to migrate to schema v7
};

GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString, sqlServerStorageOptions);
Console.WriteLine("This is Client Api");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "This is the client api");

app.MapPost("/Long", (int id, int duration) =>
{
    Console.WriteLine($"Queuing long task with Id={id} and Duration={duration}");
    BackgroundJob.Enqueue( () => SharedCode.LongTask.Execute(id, duration));
    return Guid.NewGuid();
}).WithName("QueueLong");

app.MapPost("/Short", (int id) =>
{
    Console.WriteLine($"Queuing short task with Id={id}");
    BackgroundJob.Enqueue(() => SharedCode.ShortTask.Execute(id));
    return Guid.NewGuid();
}).WithName("QueueShort");

app.Run();
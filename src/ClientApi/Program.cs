var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");

app.MapPost("/Long", async(int id, int duration) =>
{
    Console.WriteLine($"Queuing long task with Id={id} and Duration={duration}");
    await Task.Delay(200);
    return Guid.NewGuid();
})
    .WithName("QueueLong"); ;

app.Run();
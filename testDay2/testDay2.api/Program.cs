using StackExchange.Redis;
using testDay2.application.Services;
using testDay2.domain.Interfaces;
using testDay2.infrastructure.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = new ConfigurationOptions
    {
        EndPoints = { builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379" },
        AbortOnConnectFail = false
    };
    return ConnectionMultiplexer.Connect(config);
});
builder.Services.AddSingleton<IObjectRepository, RedisObjectRepository>();
builder.Services.AddSingleton<ObjectService>();

var app = builder.Build();
var objectRepo = app.Services.GetRequiredService<IObjectRepository>();
objectRepo.ObjectCreated += (sender, e) =>
{
    Console.WriteLine($"[EVENT] Created: {e.Object.Id} at ({e.Object.X},{e.Object.Y})");
};
objectRepo.ObjectUpdated += (sender, e) =>
{
    Console.WriteLine($"[EVENT] Updated: {e.OldObject.Id} → ({e.NewObject.X},{e.NewObject.Y})");
};

objectRepo.ObjectDeleted += (sender, id) =>
{
    Console.WriteLine($"[EVENT] Deleted: {id}");
};

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

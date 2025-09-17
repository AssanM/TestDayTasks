using LiteNetLib;
using StackExchange.Redis;
using tileWorld.application.Services;
using tileWorld.domain.Interfaces;
using tileWorld.infrastructure.Background;
using tileWorld.infrastructure.Network.Dispatcher;
using tileWorld.infrastructure.Network.Server;
using tileWorld.infrastructure.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ISurfaceLayer>(new SurfaceLayer(width: 1000, height: 1000));
builder.Services.AddSingleton<PathFindingQueue>();
builder.Services.AddSingleton<PathFindingWorker>();
builder.Services.AddSingleton<IObjectLayer, ObjectService>();
builder.Services.AddSingleton<IRegionLayer, RegionLayer>();

builder.Services.AddSingleton<MapUdpServer>();
builder.Services.AddSingleton<MessageDispatcher>();
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
builder.Services.AddSingleton<ObjectRepository>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
var app = builder.Build();
var udpServer = app.Services.GetRequiredService<MapUdpServer>();
udpServer.Start(9050);
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

using tileWorld.application.Services;
using tileWorld.domain.Interfaces;
using tileWorld.infrastructure.Network.Dispatcher;
using tileWorld.infrastructure.Network.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IObjectLayer, ObjectService>();
builder.Services.AddSingleton<IRegionLayer, RegionLayer>();
builder.Services.AddSingleton<MapUdpServer>();
builder.Services.AddSingleton<MessageDispatcher>();


builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
var app = builder.Build();
var udpServer = app.Services.GetRequiredService<MapUdpServer>();
udpServer.Start(9050);

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

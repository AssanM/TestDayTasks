using testDay4.application.Services;
using testDay4.dispatcher;
using testDay4.domain.Interfaces;
using testDay4.server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<ObjectService>();
builder.Services.AddSingleton<IObjectLayer>(sp => sp.GetRequiredService<ObjectService>());
builder.Services.AddSingleton<MapUdpServer>();
builder.Services.AddSingleton<MessageDispatcher>();
builder.Services.AddSingleton<IRegionLayer, RegionLayer>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
var udpServer = app.Services.GetRequiredService<MapUdpServer>();
udpServer.Start(9050);


app.Run();

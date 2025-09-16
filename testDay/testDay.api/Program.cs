using testDay.application.Interfaces;
using testDay.application.Services;
using testDay.domain.Interfaces;
using testDay.infrastructure.Background;
using testDay.infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSingleton<IEngineLayer>(new EngineLayer(width:1000, height:1000));
builder.Services.AddSingleton<PathFindingQueue>();
builder.Services.AddSingleton<PathFindingWorker>();
builder.Services.AddSingleton<ITileMapPersistence, TileMapPersistence>();

var app = builder.Build();

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

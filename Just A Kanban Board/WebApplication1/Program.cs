using Microsoft.AspNetCore.Builder;
using KanbanBoardAPI.Services;
using KanbanBoardAPI.Services.DbServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IWebAppConfig, WebAppConfig>();

builder.Services.AddSingleton<IKanbanRealmService, KanbanRealmService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

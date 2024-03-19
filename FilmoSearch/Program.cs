using System.IO;
using FilmoSearch;
using FilmoSearch.Models;
using FilmoSearch.Repositories.Actor;
using FilmoSearch.Repositories.Film;
using FilmoSearch.Repositories.Review;
using FilmoSearch.Services.Actor;
using FilmoSearch.Services.Film;
using Microsoft.EntityFrameworkCore;
using FilmoSearch.Services.Review;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

builder.Services.AddMvc();

builder.Services.AddScoped<ActorRepository>();
builder.Services.AddScoped<ActorService>();
builder.Services.AddScoped<FilmRepository>();
builder.Services.AddScoped<FilmService>();
builder.Services.AddScoped<ReviewRepository>();
builder.Services.AddScoped<ReviewService>();

Log.Logger = new LoggerConfiguration()                
                .WriteTo.Console()                
                .WriteTo.File(new JsonFormatter(),
                              "FilmoSearchImportantLogs.json",
                              restrictedToMinimumLevel: LogEventLevel.Warning)                
                .WriteTo.File("logs/FilmoSearchLogs.txt",
                              rollingInterval: RollingInterval.Day)                
                .MinimumLevel.Debug()
                .CreateLogger();

builder.Services.AddControllers();

builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
});

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

app.UseAuthorization();

app.MapControllers();

app.Run();

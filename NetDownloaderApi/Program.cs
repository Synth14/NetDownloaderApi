using NetDownloaderApi.Controllers;
using NetDownloaderApi.Interfaces;
using NetDownloaderApi.Models;
using NetDownloaderApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IDownloadService, DownloadService>();

builder.Services.AddSingleton<DownloadConfiguration>();
builder.Services.AddOptions(); 
builder.Services.Configure<DownloadConfiguration>(builder.Configuration.GetSection("DownloadConfiguration"));

builder.Configuration.AddJsonFile("appsettings.json");
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

using NetDownloaderApi.Controllers;
using NetDownloaderApi.Interfaces;
using NetDownloaderApi.Models;
using NetDownloaderApi.Services;
using Microsoft.Extensions.Configuration;
using NetDownloader.Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

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
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.Configure<DownloadConfiguration>(builder.Configuration.GetSection("DownloadConfiguration"));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.MapControllers();

app.Run();

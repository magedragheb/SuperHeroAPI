using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HeroContext>(opt => opt
                .UseSqlite("Data Source=Data/SuperHeroDB"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

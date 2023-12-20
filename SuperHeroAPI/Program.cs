using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SuperHeroAPI.Endpoints;
using SuperHeroAPI.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HeroContext>(opt => opt
                .UseSqlite("Data Source=Data/SuperHeroDB"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<AppUser>()
                .AddEntityFrameworkStores<HeroContext>();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SuperHeroAPI",
        Version = "v1",
        Description = "Reading heroes from https://superheroapi.com/ and adding to favourites per user"
    });
});

var app = builder.Build();
app.Urls.Add("https://localhost:5000");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
//map identity api endpoints
app.MapIdentityApi<AppUser>().WithTags("Identity");
//map heroes api endpoints
app.MapHeroes();

app.Run();

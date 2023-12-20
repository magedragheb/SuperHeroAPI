using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SuperHeroAPI.Endpoints;
using SuperHeroAPI.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HeroContext>(opt => opt
                .UseSqlite("Data Source=Data/SuperHeroDB"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//add authorization using Identity and its endpoints
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<AppUser>()
                .AddEntityFrameworkStores<HeroContext>();
//add httpclient to access remote api
builder.Services.AddHttpClient();
//add swagger
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
//configure Json serializer options
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.IncludeFields = true;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();
app.Urls.Add("https://localhost:5000/swagger");
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

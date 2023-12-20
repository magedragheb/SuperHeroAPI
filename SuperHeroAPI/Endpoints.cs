
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Entities;

namespace SuperHeroAPI.Endpoints;
public static class Endpoints
{
    public static RouteGroupBuilder MapHeroes(this IEndpointRouteBuilder routes)
    {
        //define group
        var group = routes.MapGroup("/heroes");
        //add tags
        group.WithTags("Heroes");
        //authorized only
        group.RequireAuthorization(a => a.RequireAuthenticatedUser());
        //external API
        //search superhero
        group.MapGet("/{name}", SearchSuperhero);
        //get hero by ID
        group.MapGet("/{heroId:int}", GetHeroById);
        //db
        //get all favourites
        group.MapGet("/", GetFavourites);
        //add favourite
        group.MapPost("/", AddFavourite);
        //delete from favourites
        group.MapDelete("/{heroId:int}", DeleteFavourite);

        return group;
    }

    /// <summary>
    /// Gets current user from httpcontext
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="db">DbContext</param>
    /// <returns>Current User or null if not found</returns>
    public static async Task<AppUser?> GetCurrentUser(HttpContext context, HeroContext db)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await db.AppUsers
                            .Where(u => u.Id == userId)
                            .Include(u => u.Heroes)
                            .FirstOrDefaultAsync();
        return user ?? null;
    }

    //use secrets
    //fix responses json

    /// <summary>
    /// Searches external api for superhero
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="name">hero name to search for</param>
    /// <returns>json object of one superhero or a list that shares the same name</returns>
    static async Task<IResult> SearchSuperhero(HttpClient httpClient, string name)
    {
        var token = "1388746048401439";
        var apiUrl = $"https://superheroapi.com/api/{token}/search/{name}";
        // Make a GET request to the external API
        var response = await httpClient.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            // Read and return the content of the response
            var content = await response.Content.ReadAsStringAsync();
            return TypedResults.Ok(content);
        }
        else
        {
            // Handle the error case
            return TypedResults.BadRequest($"Failed to fetch data. Status code: {response.StatusCode}");
        }
    }

    /// <summary>
    /// Gets hero by Id from external api
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="heroId">hero Id to search for</param>
    /// <returns>json object of one superhero</returns>
    static async Task<IResult> GetHeroById(HttpClient httpClient, int heroId)
    {
        var token = "1388746048401439";
        var apiUrl = $"https://superheroapi.com/api/{token}/{heroId}";
        // Make a GET request to the external API
        var response = await httpClient.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            // Read and return the content of the response
            var content = await response.Content.ReadAsStringAsync();
            return TypedResults.Ok(content);
        }
        else
        {
            // Handle the error case
            return TypedResults.BadRequest($"Failed to fetch data. Status code: {response.StatusCode}");
        }
    }

    /// <summary>
    /// Gets a list of user favourites
    /// </summary>
    /// <param name="db">DbContext</param>
    /// <param name="context">HttpContext</param>
    /// <returns>List of user's favourites</returns>
    static async Task<IResult> GetFavourites(HeroContext db, HttpContext context)
    {
        var currentUser = GetCurrentUser(context, db).Result;
        if (currentUser == null) return TypedResults.Unauthorized();
        return TypedResults.Ok(await db.Heroes.Where(h => currentUser.Heroes.Contains(h)).ToListAsync());
    }

    /// <summary>
    /// Adds favourite to current user
    /// </summary>
    /// <param name="db">DbContext</param>
    /// <param name="context">HttpContext</param>
    /// <param name="heroId">Id from remote Api</param>
    /// <returns></returns>
    static async Task<IResult> AddFavourite(HeroContext db, HttpContext context,
    int heroId)
    {
        var currentUser = GetCurrentUser(context, db).Result;
        if (currentUser == null) return TypedResults.Unauthorized();
        var hero = await db.Heroes.FindAsync(heroId);
        //save hero once
        if (hero == null)
        {
            var newhero = new Hero { ApiId = heroId };
            await db.Heroes.AddAsync(newhero);
            currentUser.Heroes.Add(newhero);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/heroes/{newhero.Id}", newhero);
        }
        //if already a favourite return it
        if (currentUser.Heroes.Contains(hero)) return TypedResults.Ok(hero);
        //else add it to favourites
        currentUser.Heroes.Add(hero);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/heroes/{hero.Id}", hero);
    }

    /// <summary>
    /// Removes hero from user's favourites
    /// </summary>
    /// <param name="db">DbContext</param>
    /// <param name="context">HttpContext</param>
    /// <param name="heroId">Id from remote Api</param>
    /// <returns></returns>
    static async Task<IResult> DeleteFavourite(HeroContext db, HttpContext context,
    int heroId)
    {
        var currentUser = GetCurrentUser(context, db).Result;
        if (currentUser == null) return TypedResults.Unauthorized();
        var hero = await db.Heroes.FindAsync(heroId);
        if (hero == null) return TypedResults.NotFound();
        //remove from favourites
        currentUser.Heroes.Remove(hero);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}
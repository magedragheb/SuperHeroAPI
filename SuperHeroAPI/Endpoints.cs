namespace SuperHeroAPI;
public static class Endpoints
{
    public static RouteGroupBuilder MapHeroes(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/heroes");
        group.WithTags("Heroes");

        group.RequireAuthorization(a => a.RequireAuthenticatedUser());
        //search superhero
        //get hero by ID
        //add favourite
        //delete from favourites
        //get all favourites
        group.MapGet("/", () =>
        {

        });
        return group;
    }
}

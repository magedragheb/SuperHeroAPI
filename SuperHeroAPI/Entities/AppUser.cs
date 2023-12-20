using Microsoft.AspNetCore.Identity;
namespace SuperHeroAPI.Entities;

/// <summary>
/// AppUser inherits from IdentityUser and has a list of favourite heroes
/// </summary>
public class AppUser : IdentityUser
{
    public ICollection<Hero> Heroes { get; set; } = [];
}
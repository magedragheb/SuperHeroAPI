using Microsoft.AspNetCore.Identity;
namespace SuperHeroAPI.Entities;

public class AppUser : IdentityUser
{
    public ICollection<Hero> Heroes { get; set; } = [];
}
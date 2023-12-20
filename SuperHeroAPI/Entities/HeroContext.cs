using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace SuperHeroAPI.Entities;

public class HeroContext(DbContextOptions<HeroContext> options) : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<Hero> Heroes => Set<Hero>();
}
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace SuperHeroAPI.Entities;

public class HeroContext(DbContextOptions<HeroContext> options) : IdentityDbContext(options)
{
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<Hero> Heroes => Set<Hero>();
}
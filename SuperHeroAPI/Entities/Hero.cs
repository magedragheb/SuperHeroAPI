namespace SuperHeroAPI.Entities;

public class Hero
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public int ApiId { get; set; }

    public ICollection<AppUser> Users { get; set; } = [];
}
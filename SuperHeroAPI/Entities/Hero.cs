namespace SuperHeroAPI.Entities;

/// <summary>
/// Hero entity holds hero information and saved as a user favourite
/// </summary>
public class Hero
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public int ApiId { get; set; }

    public ICollection<AppUser> AppUsers { get; set; } = [];
}
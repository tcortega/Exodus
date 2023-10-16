namespace Exodus.Api.Persistence.Entities;

public class User : BaseEntity
{
    public required string Name { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
}
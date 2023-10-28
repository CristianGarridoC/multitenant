namespace Organization.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public ICollection<UserOrganization> UserOrganization { get; set; }
}
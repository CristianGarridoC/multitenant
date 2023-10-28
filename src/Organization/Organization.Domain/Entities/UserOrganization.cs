namespace Organization.Domain.Entities;

public class UserOrganization : BaseEntity
{
    public int OrganizationId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public Organization Organization { get; set; }
}
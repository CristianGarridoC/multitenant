namespace Organization.Domain.Entities;

public class Organization : BaseEntity
{
    public string Name { get; set; }
    public string SlugTenant { get; set; }
    public ICollection<UserOrganization> UserOrganization { get; set; }
}
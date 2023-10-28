namespace Organization.Application.Organization.Queries.GetAll;

public record GetAllResult(IEnumerable<Organization> Organizations);
public record Organization(int Id, string Name, string Slug, IEnumerable<User> Users);
public record User(int Id, string Email);
namespace Organization.Application.Common.Interfaces.Authentication;

public interface IJwtProvider
{
    string Generate(Domain.Entities.User user);
}
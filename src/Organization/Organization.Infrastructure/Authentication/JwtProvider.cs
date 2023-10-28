using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Organization.Application.Common.Interfaces.Authentication;
using Organization.Application.Common.Interfaces.Services;
using Organization.Domain.Entities;
using Shared.Authentication;

namespace Organization.Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly IDateTimeService _dateTime;

    public JwtProvider(IOptions<JwtOptions> jwtOptions, IDateTimeService dateTime)
    {
        _dateTime = dateTime;
        _jwtOptions = jwtOptions.Value;
    }

    public string Generate(User user)
    {
        var claims = new Claim[]
        {
            new (JwtRegisteredClaimNames.Sid, user.Id.ToString()),
            new (JwtRegisteredClaimNames.Email, user.Email)
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            _dateTime.Now.AddHours(2),
            signingCredentials
        );

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}
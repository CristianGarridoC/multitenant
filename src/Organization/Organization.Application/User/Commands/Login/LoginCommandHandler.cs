using System.Net;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Organization.Application.Common;
using Organization.Application.Common.Data;
using Organization.Application.Common.Interfaces.Authentication;
using Organization.Application.User.Commands.Common;
using static BCrypt.Net.BCrypt;

namespace Organization.Application.User.Commands.Login;

internal sealed class LoginCommandHandler : IRequestHandler<LoginRequest, Result<GenericResponse>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(IApplicationDbContext dbContext, IJwtProvider jwtProvider)
    {
        _dbContext = dbContext;
        _jwtProvider = jwtProvider;
    }
    
    public async Task<Result<GenericResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _dbContext
            .User
            .FirstOrDefaultAsync(d => d.Email == request.Email.ToLower(), cancellationToken);
        if (user is null || !Verify(request.Password, user.Password))
        {
            return Result.Fail(new ErrorMessage(
                Constants.User.InvalidCredentials,
                HttpStatusCode.NotFound));
        }

        var token = _jwtProvider.Generate(user);
        return Result.Ok(new GenericResponse(token, user.Id));
    }
}
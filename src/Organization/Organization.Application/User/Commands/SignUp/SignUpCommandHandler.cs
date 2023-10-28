using FluentResults;
using MediatR;
using Organization.Application.Common;
using Organization.Application.Common.Data;
using Organization.Application.Common.Interfaces.Authentication;
using Organization.Application.User.Commands.Common;
using static BCrypt.Net.BCrypt;

namespace Organization.Application.User.Commands.SignUp;

internal sealed class SignUpCommandHandler : IRequestHandler<SignUpRequest, Result<GenericResponse>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IJwtProvider _jwtProvider;

    public SignUpCommandHandler(IApplicationDbContext dbContext, IJwtProvider jwtProvider)
    {
        _dbContext = dbContext;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<GenericResponse>> Handle(SignUpRequest request, CancellationToken cancellationToken)
    {
        if (DuplicatedEmail(request.Email, _dbContext))
        {
            return Result.Fail(new ErrorMessage(
                Constants.User.InvalidEmailDueToAlreadyExisting
            ));
        }
        var hashedPassword = HashPassword(request.Password);
        Domain.Entities.User user = new()
        {
            Email = request.Email.ToLower(),
            Password = hashedPassword
        };
        await _dbContext.User.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        var token = _jwtProvider.Generate(user);
        return Result.Ok(new GenericResponse(token, user.Id));
    }

    private static bool DuplicatedEmail(string email, IApplicationDbContext context)
    {
        return context.User
            .FirstOrDefault(x => x.Email == email.ToLower()) is not null;
    }
}
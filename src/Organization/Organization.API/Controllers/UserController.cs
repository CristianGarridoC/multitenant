using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organization.API.Utils;
using Organization.Application.User.Commands.Associate;
using Organization.Application.User.Commands.Login;
using Organization.Application.User.Commands.SignUp;

namespace Organization.API.Controllers;

[ApiController]
[Route("api/v1/user")]
public class UserController : ControllerBase
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] LoginRequest request)
    {
        var result = await _sender.Send(request);
        return result.ToOk();
    }
    
    [AllowAnonymous]
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
    {
        var result = await _sender.Send(request);
        return result.ToOk();
    }

    [Authorize]
    [HttpPut("tenant/association")]
    public async Task<IActionResult> Associate([FromBody] AssociateRequest request)
    {
        var result = await _sender.Send(request);
        return result.ToOk((int)HttpStatusCode.NoContent);
    }
}
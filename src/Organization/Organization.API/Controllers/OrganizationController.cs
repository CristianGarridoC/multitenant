using MediatR;
using Microsoft.AspNetCore.Mvc;
using Organization.API.Utils;
using Organization.Application.Organization.Commands.Create;
using Organization.Application.Organization.Queries.GetAll;

namespace Organization.API.Controllers
{
    [Route("api/v1/organization")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly ISender _sender;

        public OrganizationController(ISender sender)
        {
            _sender = sender;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRequest request)
        {
            var result = await _sender.Send(request);
            return result.ToOk();
        }
    
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? name)
        {
            GetAllRequest request = new(name);
            var result = await _sender.Send(request);
            return result.ToOk();
        }
    }
}

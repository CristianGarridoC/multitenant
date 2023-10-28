using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.API.Utils;
using Product.Application.Common.Data;
using Product.Application.Product.Commands.Create;
using Product.Application.Product.Commands.Delete;
using Product.Application.Product.Commands.Edit;
using Product.Application.Product.Queries.GetAll;

namespace Product.API.Controllers;

[ApiController]
[Route("api/v1/tenant")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IApplicationDbContext _dbContext;
    private readonly ISender _sender;

    public ProductController(ILogger<ProductController> logger, IApplicationDbContext dbContext, ISender sender)
    {
        _logger = logger;
        _dbContext = dbContext;
        _sender = sender;
    }

    [HttpGet("{slugTenant}/association")]
    public async Task<IActionResult> Association([FromRoute] string slugTenant)
    {
        _logger.LogInformation("Creating tenant {SlugTenant} database", slugTenant);
        await _dbContext.Database.MigrateAsync();
        return NoContent();
    }

    [HttpPost("{slugTenant}/product")]
    public async Task<IActionResult> Add([FromBody] CreateRequest request)
    {
        var result = await _sender.Send(request);
        return result.ToOk();
    }
    
    [HttpPut("{slugTenant}/product")]
    public async Task<IActionResult> Edit([FromBody] EditRequest request)
    {
        var result = await _sender.Send(request);
        return result.ToOk(204);
    }
    
    [HttpDelete("{slugTenant}/product")]
    public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
    {
        var result = await _sender.Send(request);
        return result.ToOk(204);
    }
    
    [HttpGet("{slugTenant}/product")]
    public async Task<IActionResult> GetAll([FromQuery] string? name)
    {
        GetAllRequest request = new(name);
        var result = await _sender.Send(request);
        return result.ToOk();
    }
}
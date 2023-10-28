using Product.API;
using Product.API.Middlewares;
using Product.Application;
using Product.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.AddSerilogConfiguration();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApiServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product.API V1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<CorrelationHandlingMiddleware>();
app.UseMiddleware<TenantHandlingMiddleware>();

app.MapControllers();

app.Run();
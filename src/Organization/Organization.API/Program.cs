using Organization.API;
using Organization.API.Middlewares;
using Organization.Application;
using Organization.Infrastructure;
using Organization.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.AddSerilogConfiguration();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

await app.InitialiseDatabaseAsync();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Organization.API V1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<CorrelationHandlingMiddleware>();
app.UseHeaderPropagation();
app.MapControllers();

app.Run();
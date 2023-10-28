using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Product.Infrastructure.Data;

namespace Product.API;

public class DbContextDesignFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql();

        return new ApplicationDbContext(null,
            optionsBuilder.Options,
            new HttpContextAccessor(),
            null);
    }
}
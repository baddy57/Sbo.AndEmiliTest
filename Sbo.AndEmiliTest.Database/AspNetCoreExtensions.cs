using Microsoft.Extensions.DependencyInjection;

namespace Sbo.AndEmiliTest.Database;

public static class AspNetCoreExtensions
{
    public static IServiceCollection AddAndEmiliTestDbContextAndFactory(this IServiceCollection services)
    {
        services.AddDbContextFactory<SboAndEmiliTestContext>();
        services.AddDbContext<SboAndEmiliTestContext>();
        return services;
    }
}
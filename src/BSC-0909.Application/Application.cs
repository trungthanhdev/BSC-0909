using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace BSC_0909.Application;

public static class Application
{
    public static void InjectApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}

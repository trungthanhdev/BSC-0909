using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSC_0909.Application;
using BSC_0909.Infrastructure;

namespace BSC_0909.Controller
{
    public static class Injection
    {
        public static void AddCorsConfig(this IServiceCollection service)
        {
            service.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials();
                }));
        }
        public static void AddInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.InjectApplication();
            services.AddInfrastructure(configuration);
            services.AddCorsConfig();
        }
    }
}
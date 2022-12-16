using Microsoft.Extensions.DependencyInjection;
using Assignment.Service.Interfaces;
using Assignment.Service.Services;

namespace UserManagment.Service
{
    public static class ServiceRegistration
    {
        public static void AddBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddTransient<IMasterEntryService, MasterEntryService>();
            services.ConfigureCors();
        }

        private static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .WithMethods("GET","POST")
                    .AllowAnyHeader()
                    );
            });

            return services;
        }
    }
}

using Serilog;

namespace Assignment.API
{
    public static class StartupServiceExtensions
    {
        public static void ConfigureLog(this ILoggingBuilder logging, ConfigurationManager manager)
        {
            var logger = new LoggerConfiguration().ReadFrom
                .Configuration(manager)
                .Enrich.FromLogContext()
                .CreateLogger();
            logging.ClearProviders();
            logging.AddSerilog(logger);
        }

        public static void ConfigureCustomeAuthorization(this IServiceCollection services)
        {
            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
            });
        }
    }
}

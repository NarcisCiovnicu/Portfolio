using Portfolio.API.Domain.CustomExceptions;

namespace Portfolio.API
{
    public static class PortfolioApiSetup
    {
        public static void AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            string webClientOrigin = configuration.GetValue<string>(Constants.WebClientOrigin) ?? throw new ConfigMissingException(Constants.WebClientOrigin);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policyBuilder =>
                {
                    policyBuilder.AllowAnyHeader().AllowAnyMethod();
                    policyBuilder.WithOrigins(webClientOrigin);
                });
            });
        }

        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            //
        }
    }
}

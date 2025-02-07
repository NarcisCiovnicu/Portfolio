using Common.CustomExceptions;
using Common.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.API.DataAccess.Repositories;
using Portfolio.API.Domain;
using Portfolio.API.Domain.RepositoryInterfaces;

namespace Portfolio.API.DataAccess
{
    public static class DataAccessSetup
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            string dbProvider = configuration.TryGetValue<string>(Constants.Config.DatabaseProvider);

            services.AddDbContext<PortfolioDbContext>(options => _ = dbProvider switch
            {
                "SQLServer" => options.UseSqlServer(
                    configuration.GetConnectionString(Constants.Config.PortfolioDbConnectionString),
                    opt => opt.MigrationsAssembly("Portfolio.API.DataAccess.SQLServer")),

                "SQLite" => options.UseSqlite(
                    configuration.GetConnectionString(Constants.Config.PortfolioDbConnectionString),
                    opt => opt.MigrationsAssembly("Portfolio.API.DataAccess.SQLite")),

                _ => throw new ConfigException($"Unsupported DB Provider [{dbProvider}]")
            });

            services.AddAutoMapper(typeof(PortfolioDataAccessMappingProfile));

            services.AddScoped<ITrackingRepository, TrackingRepository>();
            services.AddScoped<IPasswordRepository, PasswordRepository>();
        }


        public static void InitializeDB(IServiceProvider serviceProvider)
        {
            // Create the database/run migrations (only on debug mode)
#if DEBUG
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
            dbContext.Database.Migrate();
#endif
        }
    }
}

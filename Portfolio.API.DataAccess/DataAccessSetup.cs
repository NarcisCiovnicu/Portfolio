using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Portfolio.API.DataAccess.Repositories;
using Portfolio.API.Domain.ConfigOptions;
using Portfolio.API.Domain.CustomExceptions;
using Portfolio.API.Domain.RepositoryInterfaces;

namespace Portfolio.API.DataAccess
{
    public static class DataAccessSetup
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PortfolioDbContext>((provider, options) =>
            {
                DatabaseOptions databaseOptions = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
                string dbProvider = databaseOptions.DatabaseProvider;
                string connString = databaseOptions.ConnectionStrings.PortfolioDB;

                options = dbProvider switch
                {
                    "SQLServer" => options.UseSqlServer(connString,
                        opt =>
                        {
                            opt.MigrationsAssembly("Portfolio.API.DataAccess.SQLServer");
                            opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                            opt.EnableRetryOnFailure(maxRetryCount: 2, maxRetryDelay: TimeSpan.FromSeconds(20), errorNumbersToAdd: null);
                        }),

                    "SQLite" => options.UseSqlite(connString,
                        opt =>
                        {
                            opt.MigrationsAssembly("Portfolio.API.DataAccess.SQLite");
                            opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                        }),

                    _ => throw new ApiDbException($"Unsupported DB Provider [{dbProvider}]")
                };
            });

            services.AddAutoMapper(typeof(DataAccessMappingProfile));

            services.AddScoped<ITrackingRepository, TrackingRepository>();
            services.AddScoped<IPasswordRepository, PasswordRepository>();
            services.AddScoped<ICVRepository, CVRepository>();
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

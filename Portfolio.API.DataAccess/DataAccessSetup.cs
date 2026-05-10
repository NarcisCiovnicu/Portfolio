using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Portfolio.API.Contracts.ConfigOptions;
using Portfolio.API.Contracts.Constants;
using Portfolio.API.Contracts.CustomExceptions;
using Portfolio.API.Contracts.ProviderInterfaces;
using Portfolio.API.Contracts.RepositoryInterfaces;
using Portfolio.API.DataAccess.MappingConfigs;
using Portfolio.API.DataAccess.Providers;
using Portfolio.API.DataAccess.Repositories;

namespace Portfolio.API.DataAccess;

public static class DataAccessSetup
{
    public static void InitializeDB(IServiceProvider serviceProvider)
    {
        // Create the database/run migrations (only on debug mode)
#if DEBUG
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
        dbContext.Database.Migrate();
#endif
    }

    public static void AddServices(IServiceCollection services)
    {
        services.AddDbContext<PortfolioDbContext>((provider, options) =>
        {
            DatabaseOptions databaseOptions = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            string dbProvider = databaseOptions.DatabaseProvider;
            string connString = databaseOptions.ConnectionStrings.PortfolioDB;

            options = dbProvider switch
            {
                ConstDataProviders.SQLServer => options.UseSqlServer(connString,
                    opt =>
                    {
                        opt.MigrationsAssembly("Portfolio.API.DataAccess.SQLServer");
                        opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                        opt.EnableRetryOnFailure(maxRetryCount: 2, maxRetryDelay: TimeSpan.FromSeconds(20), errorNumbersToAdd: null);
                    }),

                ConstDataProviders.SQLite => options.UseSqlite(connString,
                    opt =>
                    {
                        opt.MigrationsAssembly("Portfolio.API.DataAccess.SQLite");
                        opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                    }),

                _ => throw new ApiDbException($"Unsupported DB Provider [{dbProvider}]")
            };
        });

        services.RegisterMappings();

        services.AddScoped<ITrackingRepository, TrackingRepository>();
        services.AddScoped<IPasswordRepository, PasswordRepository>();
        services.AddScoped<ICVRepository, CVRepository>();

        services.AddSingleton<IIpLocationProvider, IpLocationProvider>();
    }

    private static void RegisterMappings(this IServiceCollection services)
    {
        var config = new TypeAdapterConfig();
        config.Default.PreserveReference(true);

        new CurriculumVitaeMapConfigs().Register(config);
        new ApiTrackerMapConfigs().Register(config);
        new IpLocationConfigs().Register(config);

        config.Compile();

        services.AddTransient<IMapper>((_) => new Mapper(config));
    }
}

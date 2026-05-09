using Microsoft.Extensions.Options;
using Portfolio.API.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Domain.ConfigOptions;

public class DatabaseOptions
{
    [AllowedValues(ConstDataProviders.SQLite, ConstDataProviders.SQLServer,
        ErrorMessage = $"[{nameof(DatabaseProvider)}] not supported or missing from configuration.")]
    public required string DatabaseProvider { get; init; }

    [Required]
    [ValidateObjectMembers]
    public required ConnectionStrings ConnectionStrings { get; init; }
}

public class ConnectionStrings
{
    [Required]
    public required string PortfolioDB { get; init; }
}

public static class DatabaseOptionsValidation
{
    public static bool Validate(this DatabaseOptions options)
    {
        string portfolioConnString = options.ConnectionStrings.PortfolioDB;

        if (string.IsNullOrWhiteSpace(portfolioConnString))
        {
            return false;
        }

        return options.DatabaseProvider switch
        {
            ConstDataProviders.SQLite => portfolioConnString.StartsWith("Data Source="),
            ConstDataProviders.SQLServer => portfolioConnString.StartsWith("Server="),
            _ => true // Return true because of separate validation for DatabaseProvider
        };
    }

    public const string ErrorMessage = 
        $"[{nameof(DatabaseOptions.DatabaseProvider)}] and [{nameof(DatabaseOptions.ConnectionStrings)}] are not configured correctly.";
}

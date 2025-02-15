using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Domain.ConfigOptions
{
    public class DatabaseOptions
    {
        [AllowedValues("SQLite", "SQLServer", ErrorMessage = "Database provider must be one of: SQLite / SQLServer")]
        public required string DatabaseProvider { get; init; }
        [Required]
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
            if (options.DatabaseProvider == "SQLite")
            {
                return portfolioConnString.StartsWith("Data Source=");
            }
            else
            {
                //return true; // Hack - to run SQLServer migrations
                return portfolioConnString.StartsWith("Server=");
            }
        }

        public static readonly string ErrorMessage = "[DatabaseProvider] and [ConnectionStrings] are not configured correctly.";
    }
}

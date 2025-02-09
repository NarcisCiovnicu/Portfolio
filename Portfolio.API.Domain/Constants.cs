namespace Portfolio.API.Domain
{
    public static class Constants
    {
        public static class Logging
        {
            public const string ConsoleFormatterName = "ApiFormatter";
        }

        public static class IpLocationApi
        {
            public const string Name = "IpLocation";
            public const string BaseUrl = "http://ip-api.com/json/";
        }

        public static class Config
        {
            public const string Cors = "Cors";
            public const string DatabaseProvider = "DatabaseProvider";
            public const string PortfolioDbConnectionString = "PortfolioDB";
            public const string JwtToken = "JwtToken";
            public const string RateLimit = "BucketRateLimit";
        }
    }
}

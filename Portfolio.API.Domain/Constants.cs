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

        public static class DbConverter
        {
            public const char SkillsDelimiter = '•';
        }

        public static class Database
        {
            public static readonly Guid DefaultCVId = new("00000000-0000-0000-0000-000000000001");
        }

        public static class ProblemDetailsType
        {
            public const string Default = "about:blank";
            public const string Status100 = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.2";
            public const string Status200 = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.3";
            public const string Status300 = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.4";
            public const string Status400 = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5";
            public const string Status500 = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6";
        }
    }
}

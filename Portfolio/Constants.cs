namespace Portfolio
{
    public static class Constants
    {
        public const string MonthAndYearDateFormat = "MMM yyyy";

        public static class Config
        {
            public const string ClientAppConfig = "ClientAppConfig";
            public const string ApiUrl = "ApiUrl";
        }

        public static class LocalStorage
        {
            public const string CvKey = "CurriculumVitae";
            public const string IsDarkThemeKey = "IsDarkThemePreferred";
            public const string AccessTokenKey = "AccessToken";
        }

        public static class Request
        {
            public const int DefaultTimeoutSeconds = 60;
        }
    }
}

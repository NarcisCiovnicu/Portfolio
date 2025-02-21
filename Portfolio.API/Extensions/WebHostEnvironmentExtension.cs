namespace Portfolio.API.Extensions
{
    public static class WebHostEnvironmentExtension
    {
        public static bool IsStagingOrDevelopment(this IWebHostEnvironment environment)
        {
            return environment.IsDevelopment() || environment.IsStaging();
        }
    }
}

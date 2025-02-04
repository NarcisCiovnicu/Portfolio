using Common.CustomExceptions;
using Microsoft.Extensions.Configuration;

namespace Common.Utils
{
    public static class ConfigurationExtensions
    {
        public static T TryGetValue<T>(this IConfiguration configuration, string key) where T : class
        {
            return configuration.GetValue<T?>(key) ?? throw new ConfigMissingException(key);
        }

        public static T TryGetTypeValue<T>(this IConfiguration configuration, string key) where T : struct
        {
            return configuration.GetValue<T?>(key) ?? throw new ConfigMissingException(key);
        }
    }
}

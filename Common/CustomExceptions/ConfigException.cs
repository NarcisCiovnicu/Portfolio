namespace Common.CustomExceptions
{
    public class ConfigException(string message) : Exception(message) { }

    public class ConfigMissingException(string missingProperty) : ConfigException($"Config missing: {missingProperty}") { }
}

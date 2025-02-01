namespace Portfolio.Errors
{
    public class ConfigException(string message) : ClientAppException(message) { }

    public class ConfigMissingException(string missingProperty) : ConfigException($"Config missing: {missingProperty}") { }
}

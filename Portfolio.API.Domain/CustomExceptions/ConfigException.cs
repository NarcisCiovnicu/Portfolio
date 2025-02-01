namespace Portfolio.API.Domain.CustomExceptions
{
    public class ConfigException(string message) : ApiException(message) { }

    public class ConfigMissingException(string message) : ConfigException(message) { }
}

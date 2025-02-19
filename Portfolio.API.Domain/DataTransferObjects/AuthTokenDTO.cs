namespace Portfolio.API.Domain.DataTransferObjects
{
    public record AuthTokenDTO(string Value, int ExpiresInS);
}

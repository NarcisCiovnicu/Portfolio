namespace Portfolio.API.Contracts.DataTransferObjects;

public record AuthTokenDTO(string Value, DateTime ExpiresAtUtc);

namespace Portfolio.API.Contracts.RepositoryInterfaces;

public interface IPasswordRepository
{
    Task<bool> HasPassword(string hashValue, CancellationToken cancellationToken);
}

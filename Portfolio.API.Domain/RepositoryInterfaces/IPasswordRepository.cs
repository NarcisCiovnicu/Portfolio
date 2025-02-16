namespace Portfolio.API.Domain.RepositoryInterfaces
{
    public interface IPasswordRepository
    {
        Task<bool> HasPassword(string hashValue, CancellationToken cancellationToken);
    }
}

namespace Portfolio.API.Domain.ServiceInterfaces
{
    public interface IPasswordService
    {
        Task<bool> HasPassword(string password);
    }
}

using Portfolio.API.Domain.RepositoryInterfaces;
using Portfolio.API.Domain.ServiceInterfaces;
using System.Security.Cryptography;
using System.Text;

namespace Portfolio.API.AppLogic.Services
{
    internal class PasswordService(IPasswordRepository passwordRepository) : IPasswordService
    {
        private readonly IPasswordRepository _passwordRepository = passwordRepository;

        public Task<bool> HasPassword(string password)
        {
            string hashValue = HashWithSHA256(password);

            return _passwordRepository.HasPassword(hashValue);
        }

        private static string HashWithSHA256(string value)
        {
            var byteArray = SHA256.HashData(Encoding.UTF8.GetBytes(value));
            return Convert.ToHexString(byteArray);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Portfolio.API.Domain.RepositoryInterfaces;

namespace Portfolio.API.DataAccess.Repositories
{
    internal class PasswordRepository(PortfolioDbContext dbContext) : IPasswordRepository
    {
        private readonly PortfolioDbContext _dbContext = dbContext;

        public Task<bool> HasPassword(string hashValue)
        {
            return _dbContext.Passwords.AnyAsync(password => password.HashValue.ToUpper() == hashValue.ToUpper());
        }
    }
}

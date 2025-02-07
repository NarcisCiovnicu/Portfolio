using Microsoft.EntityFrameworkCore;
using Portfolio.API.DataAccess.Entities;

namespace Portfolio.API.DataAccess
{
    public class PortfolioDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<ApiTracker> ApiTrackers { get; set; }

        public DbSet<Password> Passwords { get; set; }
    }
}

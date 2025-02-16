using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Portfolio.API.DataAccess.Test
{
    public sealed class DbContextFactory : IDisposable
    {
        private DbConnection? _connection;

        private static DbContextOptions<PortfolioDbContext> CreateInMemoryContextOptions()
        {
            return new DbContextOptionsBuilder<PortfolioDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;
        }

        private DbContextOptions<PortfolioDbContext> CreateSQLiteContextOptions()
        {
            if (_connection == null)
            {
                throw new InvalidOperationException("Connection is null");
            }
            return new DbContextOptionsBuilder<PortfolioDbContext>()
                .UseSqlite(_connection).Options;
        }

        public static PortfolioDbContext CreateInMemoryDbContext()
        {
            return new PortfolioDbContext(CreateInMemoryContextOptions());
        }

        public PortfolioDbContext CreateSQLiteContext()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                using var dbContext = new PortfolioDbContext(CreateSQLiteContextOptions());
                dbContext.Database.EnsureCreated();
            }

            return new PortfolioDbContext(CreateSQLiteContextOptions());
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}

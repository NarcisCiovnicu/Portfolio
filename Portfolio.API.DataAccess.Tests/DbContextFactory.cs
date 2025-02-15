using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Portfolio.API.DataAccess.Tests
{
    public sealed class DbContextFactory : IDisposable
    {
        private DbConnection? _connection;

        public static PortfolioDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<PortfolioDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            return new PortfolioDbContext(options);
        }

        public PortfolioDbContext CreateSQLiteContext()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();
            }

            var options = new DbContextOptionsBuilder<PortfolioDbContext>()
                .UseSqlite(_connection).Options;

            using var context = new PortfolioDbContext(options);
            context.Database.EnsureCreated();

            return new PortfolioDbContext(options);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}

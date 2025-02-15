using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.API.DataAccess.Entities;

namespace Portfolio.API.DataAccess.EntityConfigurations
{
    internal class ApiTrackerConfiguration(DatabaseFacade database) : IEntityTypeConfiguration<ApiTracker>
    {
        private readonly DatabaseFacade _database = database;

        public void Configure(EntityTypeBuilder<ApiTracker> builder)
        {
            // Keep as example
            //if (_database.IsSqlite())
            //{
            //    builder.Property(p => p.Timestamp)
            //        .HasDefaultValueSql("date('now')");
            //}
            //if (_database.IsSqlServer())
            //{
            //    builder.Property(p => p.Timestamp)
            //        .HasDefaultValueSql("getdate()");
            //}
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.API.DataAccess.Entities;

namespace Portfolio.API.DataAccess.EntityConfigurations
{
    internal class WorkExperienceConfiguration : IEntityTypeConfiguration<WorkExperience>
    {
        public void Configure(EntityTypeBuilder<WorkExperience> builder)
        {
            builder.Property(p => p.EmploymentType)
                .HasConversion<string>()
                .HasMaxLength(16);

            builder.Property(p => p.LocationType)
                .HasConversion<string>()
                .HasMaxLength(16);
        }
    }
}

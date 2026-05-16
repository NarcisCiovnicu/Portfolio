using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.API.DataAccess.DatabaseEntities;

namespace Portfolio.API.DataAccess.EntityConfigurations;

internal class WorkExperienceConfiguration : IEntityTypeConfiguration<WorkExperience>
{
    public void Configure(EntityTypeBuilder<WorkExperience> builder)
    {
        builder.Property(p => p.EmploymentType)
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.Property(p => p.LocationType)
            .HasConversion<string>()
            .HasMaxLength(32);
    }
}

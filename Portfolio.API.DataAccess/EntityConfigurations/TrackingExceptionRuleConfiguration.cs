using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.API.DataAccess.DatabaseEntities;

namespace Portfolio.API.DataAccess.EntityConfigurations;

internal class TrackingExceptionRuleConfiguration : IEntityTypeConfiguration<TrackingExceptionRule>
{
    public void Configure(EntityTypeBuilder<TrackingExceptionRule> builder)
    {
        builder.Property(p => p.RuleType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.Property(p => p.Value)
            .IsRequired()
            .HasMaxLength(256);
    }
}

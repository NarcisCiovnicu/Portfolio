using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.API.DataAccess.Entities;

namespace Portfolio.API.DataAccess.EntityConfigurations;

internal class PersonalProjectConfiguration : IEntityTypeConfiguration<PersonalProject>
{
    public void Configure(EntityTypeBuilder<PersonalProject> builder)
    {
        //builder.HasOne(p => p.ExternalLink)
        //    .WithOne()
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}

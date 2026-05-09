using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Portfolio.API.DataAccess.Entities;
using Portfolio.API.Domain.Constants;

namespace Portfolio.API.DataAccess.EntityConfigurations;

internal class CurriculumVitaeConfiguration : IEntityTypeConfiguration<CurriculumVitae>
{
    public void Configure(EntityTypeBuilder<CurriculumVitae> builder)
    {
        builder.Property(p => p.Skills)
            .HasConversion(new ValueConverter<List<string>, string>(
                skills => ToString(skills),
                value => ToList(value)),
                GetSkillsValueComparer())
            .HasMaxLength(12 * 50 + 11);

        // Keep as example
        //builder.Property(p => p.Skills)
        //    .HasConversion(new ValueConverter<List<string>, string>(
        //        skills => System.Text.Json.JsonSerializer.Serialize(skills),
        //        value => System.Text.Json.JsonSerializer.Deserialize<List<string>>(value) ?? new List<string>()),
        //        GetSkillsValueComparer())
        //    .HasMaxLength(12 * (50 + 3) - 1 + 2);

        builder.HasData(new CurriculumVitae()
        {
            Id = ConstDefaults.DefaultCVId,
            Name = "Name",
            Email = "email@email",
            Location = "Location",
            Phone = "(000) 000-0000"
        });
    }

    private static string ToString(List<string> skills)
    {
        return string.Join(ConstStringDelimiters.SkillsDelimiter, skills);
    }

    private static List<string> ToList(string value)
    {
        return [.. value.Split(ConstStringDelimiters.SkillsDelimiter, StringSplitOptions.RemoveEmptyEntries)];
    }

    private static ValueComparer<List<string>> GetSkillsValueComparer()
    {
        return new ValueComparer<List<string>>(
            (s1, s2) => CompareSkills(s1, s2),
            s => s.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            s => s.ToList());
    }

    private static bool CompareSkills(IList<string>? s1, IList<string>? s2)
    {
        if (s1 == s2)
        {
            return true;
        }
        if (s1 == null || s2 == null)
        {
            return false;
        }
        return s1.SequenceEqual(s2);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Portfolio.API.DataAccess.Entities;
using Portfolio.API.Domain;
//using System.Text.Json;

namespace Portfolio.API.DataAccess.EntityConfigurations
{
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

            //builder.Property(p => p.Skills)
            //    .HasConversion(new ValueConverter<List<string>, string>(
            //        skills => Serialize(skills),
            //        value => Deserialize(value)),
            //        GetSkillsValueComparer())
            //    .HasMaxLength(12 * (50 + 3) - 1 + 2);

            builder.HasData(new CurriculumVitae()
            {
                Id = Constants.Database.DefaultCVId,
                Name = "Name",
                Email = "email@email",
                Location = "Location",
                Phone = "(000) 000-0000"
            });
        }

        private static string ToString(List<string> skills)
        {
            return string.Join(Constants.DbConverter.SkillsDelimiter, skills);
        }

        private static List<string> ToList(string value)
        {
            return [.. value.Split(Constants.DbConverter.SkillsDelimiter, StringSplitOptions.RemoveEmptyEntries)];
        }

        //private static string Serialize(List<string> skills)
        //{
        //    return JsonSerializer.Serialize(skills);
        //}

        //private static List<string> Deserialize(string value)
        //{
        //    return JsonSerializer.Deserialize<List<string>>(value) ?? [];
        //}

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
}

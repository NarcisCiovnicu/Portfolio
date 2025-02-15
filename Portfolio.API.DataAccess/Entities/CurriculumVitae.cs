using Portfolio.API.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.API.DataAccess.Entities
{
    public class CurriculumVitae : BaseEntity
    {
        [MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(30)]
        public required string Location { get; set; }

        [MaxLength(25)]
        public required string Phone { get; set; }

        [MaxLength(50)]
        public required string Email { get; set; }

        public Link? LinkedInProfile { get; set; }

        public Link? Website { get; set; }

        [MaxLength(4000)]
        public string? About { get; set; }

        public IList<WorkExperience> WorkExperienceList { get; } = [];

        public IList<PersonalProject> PersonalProjects { get; } = [];

        public IList<Education> EducationHistory { get; } = [];

        public List<string> Skills { get; } = [];
    }

    public abstract class ExternalLinkCVEntity : BaseEntity
    {
        public Link? ExternalLink { get; set; }
    }

    [Table("WorkExperiences")]
    public class WorkExperience : ExternalLinkCVEntity
    {
        [MaxLength(50)]
        public required string CompanyName { get; set; }

        [MaxLength(50)]
        public required string Title { get; set; }

        public required DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public EmploymentType? EmploymentType { get; set; }

        [MaxLength(30)]
        public string? Location { get; set; }

        public LocationType? LocationType { get; set; }

        [MaxLength(4000)]
        public string? Description { get; set; }
    }

    [Table("PersonalProjects")]
    public class PersonalProject : ExternalLinkCVEntity
    {
        [MaxLength(50)]
        public required string Title { get; set; }

        [MaxLength(4000)]
        public required string Description { get; set; }
    }

    [Table("EducationHistory")]
    public class Education : BaseEntity
    {
        [MaxLength(50)]
        public required string SchoolName { get; set; }

        [MaxLength(50)]
        public required string DegreeName { get; set; }

        public required DateOnly StartDate { get; set; }

        public required DateOnly EndDate { get; set; }
    }

    [Table("Links")]
    public class Link : BaseEntity
    {
        [MaxLength(128)]
        public required string Label { get; set; }

        [MaxLength(4000)]
        public required string Uri { get; set; }
    }
}

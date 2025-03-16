using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class CurriculumVitae
    {
        public DateOnly? LastUpdate { get; set; }

        [Length(2, 50, ErrorMessage = "Min 2 characters. Max 50 characters.")]
        public required string Name { get; set; }
        [Length(2, 30, ErrorMessage = "Min 2 characters. Max 30 characters.")]
        public required string Location { get; set; }
        [Length(9, 25, ErrorMessage = "Min 9 characters. Max 25 characters.")]
        public required string Phone { get; set; }
        [EmailAddress, MaxLength(50, ErrorMessage = "Max 50 characters.")]
        public required string Email { get; set; }
        public Link? LinkedInProfile { get; set; }
        public Link? Website { get; set; }

        [MaxLength(4000, ErrorMessage = "Max 4000 characters.")]
        public string? About { get; set; }

        [MaxLength(8, ErrorMessage = "Max 8 items.")]
        public IList<WorkExperience> WorkExperienceList { get; set; } = [];
        [MaxLength(4, ErrorMessage = "Max 4 items.")]
        public IList<PersonalProject> PersonalProjects { get; set; } = [];
        [MaxLength(3, ErrorMessage = "Max 3 items.")]
        public IList<Education> EducationHistory { get; set; } = [];

        [MaxLength(12, ErrorMessage = "Max 12 items.")]
        public IList<Skill> Skills { get; set; } = [];
    }

    public class WorkExperience
    {
        [MaxLength(50, ErrorMessage = "Max 50 characters.")]
        public required string CompanyName { get; set; }
        [MaxLength(50, ErrorMessage = "Max 50 characters.")]
        public required string Title { get; set; }
        public required DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public EmploymentType? EmploymentType { get; set; }
        [MaxLength(30, ErrorMessage = "Max 30 characters.")]
        public string? Location { get; set; }
        public LocationType? LocationType { get; set; }
        [MaxLength(4000, ErrorMessage = "Max 4000 characters.")]
        public string? Description { get; set; }
        public Link? ExternalLink { get; set; }
    }

    public class PersonalProject
    {
        [MaxLength(50, ErrorMessage = "Max 50 characters.")]
        public required string Title { get; set; }
        [MaxLength(4000, ErrorMessage = "Max 4000 characters.")]
        public required string Description { get; set; }
        public Link? ExternalLink { get; set; }
    }

    public class Education
    {
        [MaxLength(50, ErrorMessage = "Max 50 characters.")]
        public required string SchoolName { get; set; }
        [MaxLength(50, ErrorMessage = "Max 50 characters.")]
        public required string DegreeName { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
    }

    public class Link
    {
        [MaxLength(128, ErrorMessage = "Max 128 characters.")]
        public required string Label { get; set; }
        [Url, MaxLength(4000, ErrorMessage = "Max 4000 characters.")]
        public required string Uri { get; set; }
    }

    public class Skill
    {
        [Required, MaxLength(50), DeniedValues('•')]
        public required string Name { get; set; }
    }

    public enum EmploymentType
    {
        None = 0,
        [Description("Full-time")]
        FullTime = 1,
        [Description("Part-time")]
        PartTime = 2,
        Contract = 3,
        Internship = 4,
        Freelance = 5,
    }

    public enum LocationType
    {
        None = 0,
        [Description("On-site")]
        OnSite = 1,
        Hybrid = 2,
        Remote = 3,
    }
}

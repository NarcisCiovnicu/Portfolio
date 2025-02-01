using System.ComponentModel;

namespace Portfolio.Models
{
    public class CurriculumVitae
    {
        public DateOnly? LastUpdate { get; set; }

        public required string Name { get; set; }
        public required string Location { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public Link? LinkedInProfile { get; set; }
        public Link? Website { get; set; }

        public string? About { get; set; }

        public IList<WorkExperience>? WorkExperienceList { get; set; }
        public IList<PersonalProject>? PersonalProjects { get; set; }
        public IList<Education>? EducationHistory { get; set; }

        public IList<string>? Skills { get; set; }
    }

    public class WorkExperience
    {
        public required string CompanyName { get; set; }
        public required string Title { get; set; }
        public required DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public EmploymentType? EmploymentType { get; set; }
        public string? Location { get; set; }
        public LocationType? LocationType { get; set; }
        public string? Description { get; set; }
        public Link? ExternalLink { get; set; }
    }

    public class PersonalProject
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public Link? ExternalLink { get; set; }
    }

    public class Education
    {
        public required string SchoolName { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
    }

    public class Link
    {
        public required string Label { get; set; }
        public required string Uri { get; set; }
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

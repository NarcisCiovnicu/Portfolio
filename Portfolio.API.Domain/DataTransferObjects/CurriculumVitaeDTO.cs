using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Domain.DataTransferObjects
{
    public record CurriculumVitaeDTO(
        [Required, MaxLength(50)] string Name,
        [Required, MaxLength(30)] string Location,
        [Required, MaxLength(25)] string Phone,
        [Required, EmailAddress, MaxLength(50)] string Email,
        LinkDTO? LinkedInProfile,
        LinkDTO? Website,
        [MaxLength(4000)] string? About,
        [MaxLength(8)] IList<WorkExperienceDTO> WorkExperienceList,
        [MaxLength(4)] IList<PersonalProjectDTO> PersonalProjects,
        [MaxLength(3)] IList<EducationDTO> EducationHistory,
        [MaxLength(12)] IList<SkillDTO> Skills
    );

    public record LinkDTO(
        [Required, MaxLength(128)] string Label,
        [Required, Url, MaxLength(4000)] string Uri
    );

    public abstract record ExternalLinkCVDTO(LinkDTO? ExternalLink);

    public record WorkExperienceDTO(
        [Required, MaxLength(50)] string CompanyName,
        [Required, MaxLength(50)] string Title,
        DateOnly StartDate,
        DateOnly? EndDate,
        EmploymentType? EmploymentType,
        [MaxLength(30)] string? Location,
        LocationType? LocationType,
        [MaxLength(4000)] string? Description,
        LinkDTO? ExternalLink
    ) : ExternalLinkCVDTO(ExternalLink);

    public record PersonalProjectDTO(
        [Required, MaxLength(50)] string Title,
        [Required, MaxLength(4000)] string Description,
        LinkDTO? ExternalLink
    ) : ExternalLinkCVDTO(ExternalLink);

    public record EducationDTO(
        [Required, MaxLength(50)] string SchoolName,
        [Required, MaxLength(50)] string DegreeName,
        DateOnly StartDate,
        DateOnly EndDate
    );

    public record SkillDTO([Required, MaxLength(50), DeniedValues(Constants.DbConverter.SkillsDelimiter)] string Name);

    public enum EmploymentType
    {
        None = 0,
        FullTime = 1,
        PartTime = 2,
        Contract = 3,
        Internship = 4,
        Freelance = 5,
    }

    public enum LocationType
    {
        None = 0,
        OnSite = 1,
        Hybrid = 2,
        Remote = 3,
    }
}

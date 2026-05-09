using Mapster;
using Portfolio.API.DataAccess.Entities;
using Portfolio.API.Contracts.DataTransferObjects;

namespace Portfolio.API.DataAccess.MappingConfigs;

public class CurriculumVitaeMapConfigs : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, SkillDTO>()
            .MapWith(src => new SkillDTO(src));

        config.NewConfig<LinkDTO, Link>().TwoWays();

        config.NewConfig<WorkExperienceDTO, WorkExperience>().TwoWays();

        config.NewConfig<PersonalProjectDTO, PersonalProject>().TwoWays();

        config.NewConfig<EducationDTO, Education>().TwoWays();

        config.NewConfig<CurriculumVitaeDTO, CurriculumVitae>()
            .Ignore(dest => dest.WorkExperienceList)
            .Ignore(dest => dest.PersonalProjects)
            .Ignore(dest => dest.EducationHistory)
            .AfterMapping((src, dest) =>
            {
                dest.Skills.Clear();
                foreach (var skill in src.Skills)
                {
                    dest.Skills.Add(skill.Name);
                }
            })
            .TwoWays();
    }
}

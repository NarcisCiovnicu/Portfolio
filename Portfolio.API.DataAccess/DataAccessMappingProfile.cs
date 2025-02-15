using AutoMapper;
using Portfolio.API.DataAccess.Entities;
using Portfolio.API.Domain.DataTransferObjects;

namespace Portfolio.API.DataAccess
{
    internal class DataAccessMappingProfile : Profile
    {
        public DataAccessMappingProfile()
        {
            CreateMap<ApiTrackerDTO, ApiTracker>();

            CreateMap<SkillDTO, string>().ConvertUsing(dto => dto.Name);

            CreateMap<LinkDTO, Link>().ReverseMap();

            CreateMap<WorkExperienceDTO, WorkExperience>().ReverseMap();

            CreateMap<PersonalProjectDTO, PersonalProject>().ReverseMap();

            CreateMap<EducationDTO, Education>().ReverseMap();

            CreateMap<CurriculumVitaeDTO, CurriculumVitae>()
                .ForMember(dto => dto.WorkExperienceList, opt => opt.Ignore())
                .ForMember(dto => dto.PersonalProjects, opt => opt.Ignore())
                .ForMember(dto => dto.EducationHistory, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}

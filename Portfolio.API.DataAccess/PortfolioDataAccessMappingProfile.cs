using AutoMapper;
using Portfolio.API.DataAccess.Entities;
using Portfolio.API.Domain.DataTransferObjects;

namespace Portfolio.API.DataAccess
{
    internal class PortfolioDataAccessMappingProfile : Profile
    {
        public PortfolioDataAccessMappingProfile()
        {
            CreateMap<ApiTrackerDTO, ApiTracker>();
        }
    }
}

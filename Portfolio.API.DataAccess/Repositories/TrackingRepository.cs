using AutoMapper;
using Portfolio.API.DataAccess.Entities;
using Portfolio.API.Domain.DataTransferObjects;
using Portfolio.API.Domain.RepositoryInterfaces;

namespace Portfolio.API.DataAccess.Repositories
{
    internal class TrackingRepository(PortfolioDbContext dbContext, IMapper mapper) : ITrackingRepository
    {
        private readonly PortfolioDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        async Task ITrackingRepository.Create(ApiTrackerDTO apiTrackerDto)
        {
            ApiTracker apiTracker = _mapper.Map<ApiTracker>(apiTrackerDto);
            _dbContext.ApiTrackers.Add(apiTracker);
            await _dbContext.SaveChangesAsync();
        }
    }
}

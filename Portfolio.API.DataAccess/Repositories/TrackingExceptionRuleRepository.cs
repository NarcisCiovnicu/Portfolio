using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.Contracts.DataTransferObjects;
using Portfolio.API.Contracts.RepositoryInterfaces;
using Portfolio.API.DataAccess.DatabaseEntities;

namespace Portfolio.API.DataAccess.Repositories;

internal class TrackingExceptionRuleRepository(PortfolioDbContext dbContext, IMapper mapper) : ITrackingExceptionRuleRepository
{
    private readonly PortfolioDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public Task<List<TrackingExceptionRuleDTO>> GetAllRules()
    {
        return _dbContext.TrackingExceptionRules.AsNoTracking().ProjectToType<TrackingExceptionRuleDTO>(_mapper.Config).ToListAsync();
    }

    public Task Add(TrackingExceptionRuleDTO ruleDTO)
    {
        var rule = _mapper.Map<TrackingExceptionRule>(ruleDTO);
        _dbContext.TrackingExceptionRules.Add(rule);

        return _dbContext.SaveChangesAsync();
    }
}

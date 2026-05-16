using Mapster;
using Portfolio.API.Contracts.DataTransferObjects;
using Portfolio.API.DataAccess.DatabaseEntities;

namespace Portfolio.API.DataAccess.MappingConfigs;

internal class ApiTrackerMapConfigs : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ApiTrackerDTO, ApiTracker>();

        config.NewConfig<TrackingExceptionRuleDTO, TrackingExceptionRule>()
            .TwoWays();
    }
}

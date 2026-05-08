using Mapster;
using Portfolio.API.DataAccess.Entities;
using Portfolio.API.Domain.DataTransferObjects;

namespace Portfolio.API.DataAccess.MappingConfigs;

public class ApiTrackerMapConfigs : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ApiTrackerDTO, ApiTracker>();
    }
}

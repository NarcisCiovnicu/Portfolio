using Mapster;
using Portfolio.API.Contracts.DataTransferObjects;
using Portfolio.API.DataAccess.ProviderModels;

namespace Portfolio.API.DataAccess.MappingConfigs;

internal class IpLocationConfigs : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<IpLocationResponse, IpLocationDTO>();
    }
}

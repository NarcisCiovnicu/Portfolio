using Mapster;
using MapsterMapper;

namespace Portfolio.API.DataAccess.Test.TestHelpers;

internal static class MapperUtils
{
    public static IMapper CreateMapper(params IRegister[] registers)
    {
        var config = new TypeAdapterConfig();
        config.Default.PreserveReference(true);

        foreach (var register in registers)
        {
            register.Register(config);
        }

        config.Compile();
        
        return new Mapper(config);
    }
}

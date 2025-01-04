using AutoMapper;

namespace BakeryApp.Application.Mappers
{
    public static class BakeryMapper
    {
        public static Lazy<IMapper> mapper = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration((cfg) =>
            {
                cfg.ShouldMapField = propertyInfo => propertyInfo.IsPublic || propertyInfo.IsAssembly;
                cfg.AddProfile<BakeryMappingProfile>();
            });
            return config.CreateMapper();
        });
        public static IMapper Mapper = mapper.Value;

    }
}

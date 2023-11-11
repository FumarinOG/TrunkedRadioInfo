using AutoMapper;
using DataLibrary;
using ServiceCommon;

namespace ServiceLibrary
{
    public static class Config
    {
        public static IMapper AutoMapperSetup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FilterDataModel, FilterData>();
            });

            config.AssertConfigurationIsValid();
            return config.CreateMapper();
        }
    }
}

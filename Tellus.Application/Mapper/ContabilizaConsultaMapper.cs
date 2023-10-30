using AutoMapper;
using Tellus.Application.Responses;
using Tellus.Domain.Models;

namespace Tellus.Application.Mapper
{
    public class ContabilizaConsultaMapper<T> where T : class
    {
        public static T Map(object source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ContabilizaConsulta, ContabilizaConsultaResponse>().ReverseMap();
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<T>(source);
        }
    }
}

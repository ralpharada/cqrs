using AutoMapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Domain.Models;

namespace Tellus.Application.Mapper
{
    public class VinculoMapper<T> where T : class
    {
        public static T Map(object source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Vinculo, AdicionarVinculoQuery>().ReverseMap();
                cfg.CreateMap<Vinculo, AtualizarVinculoQuery>().ReverseMap();
                cfg.CreateMap<Vinculo, VinculoResponse>().ReverseMap();
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<T>(source);
        }
    }
}

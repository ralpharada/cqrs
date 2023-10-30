using AutoMapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Domain.Enums;
using Tellus.Domain.Models;

namespace Tellus.Application.Mapper
{
    public class IndiceMapper<T> where T : class
    {
        public static T Map(object source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Indice, AdicionarIndiceQuery>().ReverseMap();
                cfg.CreateMap<Indice, AtualizarIndiceQuery>().ReverseMap();
                cfg.CreateMap<Indice, IndiceResponse>().ReverseMap();
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<T>(source);
        }
    }
}

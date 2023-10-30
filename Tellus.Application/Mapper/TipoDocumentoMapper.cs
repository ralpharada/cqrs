using AutoMapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Domain.Models;

namespace Tellus.Application.Mapper
{
    public class TipoDocumentoMapper<T> where T : class
    {
        public static T Map(object source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Indice, IndiceResponse>().ReverseMap();
                cfg.CreateMap<TipoDocumento, AdicionarTipoDocumentoQuery>().ReverseMap();
                cfg.CreateMap<TipoDocumento, AtualizarTipoDocumentoQuery>().ReverseMap();
                cfg.CreateMap<TipoDocumento, TipoDocumentoResponse>().ReverseMap();
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<T>(source);
        }
    }
}

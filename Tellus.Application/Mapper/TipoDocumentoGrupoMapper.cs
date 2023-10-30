using AutoMapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Domain.Models;

namespace Tellus.Application.Mapper
{
    public class TipoDocumentoGrupoMapper<T> where T : class
    {
        public static T Map(object source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Indice, IndiceResponse>().ReverseMap();
                cfg.CreateMap<TipoDocumentoGrupo, AdicionarTipoDocumentoGrupoQuery>().ReverseMap();
                cfg.CreateMap<TipoDocumentoGrupo, AtualizarTipoDocumentoGrupoQuery>().ReverseMap();
                cfg.CreateMap<TipoDocumento, TipoDocumentoResponse>().ReverseMap();
                cfg.CreateMap<TipoDocumentoGrupo, TipoDocumentoGrupoResponse>().ReverseMap();
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<T>(source);
        }
    }
}

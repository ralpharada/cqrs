using AutoMapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Domain.Models;

namespace Tellus.Application.Mapper
{
    public class DocumentoMapper<T> where T : class
    {
        public static T Map(object source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Documento, AdicionarDocumentoQuery>().ReverseMap();
                cfg.CreateMap<TipoDocumento, TipoDocumentoResponse>().ReverseMap();
                cfg.CreateMap<Indice, IndiceResponse>().ReverseMap();
                cfg.CreateMap<IndiceValor, IndiceValorResponse>().ReverseMap();
                cfg.CreateMap<Documento, DocumentoResponse>().ReverseMap();
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<T>(source);
        }
    }
}

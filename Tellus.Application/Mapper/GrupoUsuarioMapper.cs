using AutoMapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Domain.Models;

namespace Tellus.Application.Mapper
{
    public class GrupoUsuarioMapper<T> where T : class
    {
        public static T Map(object source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Indice, IndiceResponse>().ReverseMap();
                cfg.CreateMap<GrupoUsuario, AdicionarGrupoUsuarioQuery>().ReverseMap();
                cfg.CreateMap<GrupoUsuario, AtualizarGrupoUsuarioQuery>().ReverseMap();
                cfg.CreateMap<TipoDocumento, TipoDocumentoResponse>().ReverseMap();
                cfg.CreateMap<Usuario, UsuarioResponse>().ReverseMap();
                cfg.CreateMap<Permissao, PermissaoResponse>().ReverseMap();
                cfg.CreateMap<GrupoUsuario, GrupoUsuarioResponse>().ReverseMap();
                cfg.CreateMap<VinculoPermissao, VinculoPermissaoResumidoResponse>().ReverseMap();
                cfg.CreateMap<Produto, ProdutoResponse>().ReverseMap();
                cfg.CreateMap<GrupoUsuario, GrupoUsuarioPermissaoResponse>().ReverseMap();

            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<T>(source);
        }
    }
}

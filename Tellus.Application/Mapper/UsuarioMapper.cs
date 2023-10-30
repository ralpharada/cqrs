using AutoMapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Domain.Models;

namespace Tellus.Application.Mapper
{
    public class UsuarioMapper<T> where T : class
    {
        public static T Map(object source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Usuario, AdicionarUsuarioQuery>().ReverseMap();
                cfg.CreateMap<Usuario, AtualizarUsuarioQuery>().ReverseMap();
                cfg.CreateMap<Produto, ProdutoResponse>().ReverseMap();
                cfg.CreateMap<Usuario, UsuarioResponse>().ReverseMap();
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<T>(source);
        }
    }
}

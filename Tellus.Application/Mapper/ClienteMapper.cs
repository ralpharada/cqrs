using AutoMapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Domain.Models;

namespace Tellus.Application.Mapper
{
    public class ClienteMapper<T> where T : class
    {
        public static T Map(object source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Cliente, AdicionarClienteQuery>().ReverseMap();
                cfg.CreateMap<Cliente, AtualizarClienteQuery>().ReverseMap();
                cfg.CreateMap<Produto, ProdutoResponse>().ReverseMap();
                cfg.CreateMap<Cliente, ClienteResponse>().ReverseMap();
                cfg.CreateMap<Cliente, ClienteLogadoResponse>().ReverseMap();
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<T>(source);
        }
    }
}

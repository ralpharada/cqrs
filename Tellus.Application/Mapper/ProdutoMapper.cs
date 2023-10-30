using AutoMapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Domain.Models;

namespace Tellus.Application.Mapper
{
    public class ProdutoMapper<T> where T : class
    {
        public static T Map(object source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Produto, AdicionarProdutoQuery>().ReverseMap();
                cfg.CreateMap<Produto, AtualizarProdutoQuery>().ReverseMap();
                cfg.CreateMap<Produto, ProdutoResponse>().ReverseMap();
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<T>(source);
        }
    }
}

using AutoMapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Domain.Models;

namespace Tellus.Application.Mapper
{
    public class PermissaoMapper<T> where T : class
    {
        public static T Map(object source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Permissao, AdicionarPermissaoQuery>().ReverseMap();
                cfg.CreateMap<Permissao, AtualizarPermissaoQuery>().ReverseMap();
                cfg.CreateMap<Permissao, PermissaoResponse>().ReverseMap();
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<T>(source);
        }
    }
}

using AutoMapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Domain.Models;

namespace Tellus.Application.Mapper
{
    public class UsuarioAdmMapper<T> where T : class
    {
        public static T Map(object source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UsuarioAdm, AdicionarUsuarioAdmQuery>().ReverseMap();
                cfg.CreateMap<UsuarioAdm, AtualizarUsuarioAdmQuery>().ReverseMap();
                cfg.CreateMap<UsuarioAdm, UsuarioAdmResponse>().ReverseMap();
                cfg.CreateMap<UsuarioAdm, UsuarioAdmLogadoResponse>().ReverseMap();
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<T>(source);
        }
    }
}

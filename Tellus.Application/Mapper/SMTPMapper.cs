using AutoMapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Domain.Models;

namespace Tellus.Application.Mapper
{
    public class SMTPMapper<T> where T : class
    {
        public static T Map(object source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SMTP, AdicionarSMTPQuery>().ReverseMap();
                cfg.CreateMap<SMTP, AtualizarSMTPQuery>().ReverseMap();
                cfg.CreateMap<SMTP, SMTPResponse>().ReverseMap();
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<T>(source);
        }
    }
}

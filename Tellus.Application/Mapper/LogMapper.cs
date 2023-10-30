using AutoMapper;
using Tellus.Application.Responses;
using Tellus.Domain.Models;

namespace Tellus.Application.Mapper
{
    public class LogMapper<T> where T : class
    {
        public static T Map(object source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Log, LogResponse>().ReverseMap();
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<T>(source);
        }
    }
}

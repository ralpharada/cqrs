using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AtualizarTipoDocumentoQuery : IRequest<IEvent>
    {
        public Guid Id { get; private set; }
        public string Nome { get; set; } = null!;
        public DateTime DataUltimaAlteracao { get; set; }
        public bool Status { get; set; }
        public AtualizarTipoDocumentoQuery(Guid id, string nome, bool status)
        {
            Id = id;
            Nome = nome;
            Status = status;
            DataUltimaAlteracao = DateTime.UtcNow;
        }
    }
}

using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AtualizarIndiceQuery : IRequest<IEvent>
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; } = null!;
        public string TipoIndice { get; private set; }
        public string Tamanho { get; private set; } = null!;
        public string Mascara { get; private set; } = null!;
        public bool Obrigatorio { get; private set; }
        public List<String>? Lista { get; private set; }
        public AtualizarIndiceQuery(Guid id,  string nome, string tipoIndice, string tamanho, string mascara, bool obrigatorio, List<String>? lista)
        {
            Id = id;
            Nome = nome;
            TipoIndice = tipoIndice;
            Tamanho = tamanho;
            Mascara = mascara;
            Obrigatorio = obrigatorio;
            Lista = lista;
        }
    }
}

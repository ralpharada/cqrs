using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class PesquisarDocumentosExcluidosQuery : IRequest<IEvent>
    {
        public int CurrentPage { get; private set; }

        public PesquisarDocumentosExcluidosQuery(int currentPage)
        {
            CurrentPage = currentPage;
        }
    }
}

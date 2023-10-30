namespace Tellus.Domain.Interfaces
{
    public interface IGerenciaArquivoRepository
    {
        void CriarContainer(string nomeContainer);
        Task<string> SubirArquivo(string nomeArquivo, Stream content, CancellationToken cancellationToken);
        void Baixar(string nomeArquivo);
        Task<bool> Deletar(string nomeArquivo, CancellationToken cancellationToken);
    }
}

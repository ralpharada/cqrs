namespace Tellus.Application.Responses
{
    public class VinculoPermissaoResponse
    {
        public VinculoResponse Vinculo { get; set; } = null!;
        public List<PermissaoResponse> Permissoes { get; set; } = null!;
    }
}

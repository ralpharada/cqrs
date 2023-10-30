using System.Text;

namespace Tellus.Application.Util
{
    public class GeradorSenha
    {
        private static readonly Random _random = new();
        private static int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
        private static string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length = 26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
        public static string Gerar(int tamanho)
        {
            int tamanhoDividido = tamanho / 3;
            var passwordBuilder = new StringBuilder();

            // 4-Letters lower case   
            passwordBuilder.Append(RandomString(tamanhoDividido, true));

            int tamanhoInicial = Convert.ToInt16("1".PadRight(tamanhoDividido, '0'));
            int tamanhoFinal = Convert.ToInt16("9".PadRight(tamanhoDividido, '9'));
            passwordBuilder.Append(RandomNumber(tamanhoInicial, tamanhoFinal));
            var tamamhoRestante = tamanho - passwordBuilder.ToString().Length;
            // 2-Letters upper case  
            passwordBuilder.Append(RandomString(tamamhoRestante));
            var novaSenha = String.Join("", passwordBuilder.ToString().ToArray().OrderBy(x => _random.Next()).ToArray());
            return novaSenha;
        }
    }
}

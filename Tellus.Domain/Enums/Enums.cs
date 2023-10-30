using System.ComponentModel;

namespace Tellus.Domain.Enums
{
    public enum ETipoUsuario
    {
        UsuarioAdm = 1,
        Usuario = 2,
    }
    public enum ETipoResponsavel
    {
        [Description("Contratação")]
        Contratacao = 1,
        [Description("Financeiro")]
        Financeiro = 2,
        [Description("Técnico")]
        Tecnico = 3
    }
    public enum ETipoIndice
    {
        [Description("Caractere")]
        Caractere = 1,
        [Description("Número")]
        Numero = 2,
        [Description("Booleano")]
        Booleano = 3,
        [Description("Data")]
        Data = 4,
        [Description("Hora")]
        Hora = 5,
        [Description("Decimal")]
        Decimal = 6,
        [Description("Lista")]
        Lista = 7
    }
    public static class EnumEx
    {
        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
            // Or return default(T);
        }
    }
}

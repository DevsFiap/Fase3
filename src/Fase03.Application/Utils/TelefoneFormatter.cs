namespace Fase03.Application.Utils;

public static class TelefoneFormatter
{
    public static string FormatarTelefone(string telefone)
    {
        // Validação do telefone
        if (string.IsNullOrWhiteSpace(telefone) || telefone.Length < 10 || telefone.Length > 11)
            throw new ArgumentException("Número de telefone inválido.");

        var ddd = telefone.Substring(0, 2);
        var numeroTelefone = telefone.Substring(2);

        // Lógica de formatação
        if (numeroTelefone.Length == 9) // Telefone celular
            return $"({ddd}) {numeroTelefone.Substring(0, 5)}-{numeroTelefone.Substring(5)}";
        else if (numeroTelefone.Length == 8) // Telefone fixo
            return $"({ddd}) {numeroTelefone.Substring(0, 4)}-{numeroTelefone.Substring(4)}";

        // Retornar um erro caso o número não tenha o comprimento esperado
        throw new ArgumentException("Número de telefone inválido.");
    }

    public static void ValidarTelefone(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone) || telefone.Length < 10 || telefone.Length > 11)
        {
            throw new ArgumentException("Telefone deve conter DDD + número com 10 ou 11 dígitos.");
        }
    }
}
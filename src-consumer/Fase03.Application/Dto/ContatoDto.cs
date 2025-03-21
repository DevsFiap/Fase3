using System.Text.Json.Serialization;
using Fase03.Application.Utils;
using Fase03.Domain.Enums;

namespace Fase03.Application.Dto;

public class ContatoDto
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public EnumDDD DDDTelefone { get; set; }
    public string? NumeroTelefone { get; set; }
    public DateTime DataCriacao { get; set; }

    [JsonIgnore]
    public string TelefoneFormatado => TelefoneFormatter.FormatarTelefone(NumeroTelefone);

    public void SepararTelefone(string telefoneCompleto)
    {
        if (telefoneCompleto == null)
            throw new ArgumentNullException(nameof(telefoneCompleto));

        DDDTelefone = (EnumDDD)int.Parse(telefoneCompleto.Substring(0, 2));
        NumeroTelefone = telefoneCompleto.Substring(2);
    }
}
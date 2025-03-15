using Fase03.Domain.Enums;

namespace Fase03.Application.Dto;

public class ContatoDto
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public EnumDDD DDDTelefone { get; set; }
    public string? NumeroTelefone { get; set; }
    public DateTime DataCriacao { get; set; }

}
using Fase03.Domain.Enums;

namespace Fase03.Domain.Entities;

public class Contato 
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public EnumDDD DDDTelefone { get; set; }
    public DateTime DataCriacao { get; set; }
}
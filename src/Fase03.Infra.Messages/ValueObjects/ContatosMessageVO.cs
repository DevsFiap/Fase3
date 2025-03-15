namespace Fase03.Infra.Message.ValueObjects;

/// <summary>
/// Objeto de valor para gravar dados de contato na mensagem da fila
/// </summary>
public class ContatosMessageVO
{
    public int? Id { get; set; }
    public string? Nome { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
}
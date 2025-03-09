namespace Fase03.Infra.Message.ValueObjects;

/// <summary>
/// Objeto de valor para gravar dados de usuário na mensagem da fila
/// </summary>
public class UsuariosMessageVO
{
    public Guid? Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
}
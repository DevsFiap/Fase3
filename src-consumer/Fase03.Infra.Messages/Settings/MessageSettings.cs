namespace Fase03.Infra.Message.Settings;

/// <summary>
/// Configurações para conexão no servidor de mensageria
/// </summary>
public class MessageSettings
{
    public string? Host { get; set; }
    public string? Queue { get; set; }
}
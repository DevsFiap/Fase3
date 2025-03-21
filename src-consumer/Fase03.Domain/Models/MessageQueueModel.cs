namespace Fase03.Domain.Models
{
    /// <summary>
    /// Classe para modelar as mensagens que serão escritas na fila
    /// </summary>
    public class MessageQueueModel
    {
        /// <summary>
        /// Identificador da mensagem na fila
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Tipo da mensagem na fila
        /// </summary>
        public TipoMensagem? Tipo { get; set; }

        /// <summary>
        /// Conteúdo da mensagem na fila
        /// </summary>
        public string? Conteudo { get; set; }

        /// <summary>
        /// Data e hora de escrita da mensagem na fila
        /// </summary>
        public DateTime? DataHoraCriacao { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Tipo de mensagem gravada na fila
    /// </summary>
    public enum TipoMensagem
    {
        INSERIR_CONTATO = 1,
        ATUALIZAR_CONTATO = 2,
        DELETAR_CONTATO
    }
}

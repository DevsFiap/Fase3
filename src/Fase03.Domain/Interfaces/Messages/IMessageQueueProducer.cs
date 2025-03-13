using Fase03.Domain.Models;

namespace Fase03.Domain.Interfaces.Messages
{
    public interface IMessageQueueProducer
    {
        void Create(MessageQueueModel model);
    }
}

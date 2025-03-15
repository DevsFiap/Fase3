using Fase03.Domain.Core;
using Fase03.Domain.Entities;
using Fase03.Domain.Enums;

namespace Fase03.Domain.Interfaces.Repositories;

public interface IContatosRepository : IBaseRepository<Contato>
{
    Task<IEnumerable<Contato>> GetByDDDAsync(EnumDDD ddd);
}
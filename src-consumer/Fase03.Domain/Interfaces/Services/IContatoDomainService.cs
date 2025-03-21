using Fase03.Domain.Entities;
using Fase03.Domain.Enums;

namespace Fase03.Domain.Interfaces.Services;

public interface IContatoDomainService
{
    Task<List<Contato>> BuscarContatos();
    Task<Contato> GetByIdAsync(int id);
    Task<IEnumerable<Contato>> GetByDDDAsync(EnumDDD ddd);
    Task CreateContatoAsync(Contato contato);
    Task UpdateContatoAsync(int id, Contato contato);
    Task DeleteContatoAsync(int id);
}
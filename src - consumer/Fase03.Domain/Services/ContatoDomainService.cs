using Fase03.Domain.Entities;
using Fase03.Domain.Enums;
using Fase03.Domain.Exceptions;
using Fase03.Domain.Interfaces.Repositories;
using Fase03.Domain.Interfaces.Services;

namespace Fase03.Domain.Services;

public class ContatoDomainService : IContatoDomainService
{
    private readonly IContatosRepository _contatoRepository;

    public ContatoDomainService(IContatosRepository contatoRepository)
    {
        _contatoRepository = contatoRepository;
    }     

    public async Task<List<Contato>> BuscarContatos()
        => await _contatoRepository.GetAllAsync();

    public async Task<Contato> GetByIdAsync(int id)
        => await _contatoRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Contato>> GetByDDDAsync(EnumDDD ddd)
    {
        if (!Enum.IsDefined(typeof(EnumDDD), ddd))
            throw new DDDException($"O DDD '{ddd}' não é válido.");

        return await _contatoRepository.GetByDDDAsync(ddd);
    }

    public async Task CreateContatoAsync(Contato contato)
    {
         await _contatoRepository.CreateAsync(contato);
    }

    public async Task UpdateContatoAsync(int id, Contato contato)
    {
        var existingContato = await _contatoRepository.GetByIdAsync(id);
        if (existingContato == null)
            throw new DomainException($"Contato com ID '{id}' não encontrado.");

        await _contatoRepository.UpdateAsync(existingContato);
    }

    public async Task DeleteContatoAsync(int id)
    {
        var contato = await _contatoRepository.GetByIdAsync(id);
        if (contato == null)
            throw new DomainException($"Contato com ID '{id}' não encontrado.");

        await _contatoRepository.DeleteAsync(contato);
    }
}
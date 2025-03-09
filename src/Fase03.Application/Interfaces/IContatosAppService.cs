using Fase03.Application.Commands;
using Fase03.Application.Dto;
using Fase03.Domain.Enums;

namespace Fase03.Application.Interfaces;

public interface IContatosAppService
{
    Task<List<ContatoDto>> GetContatos();
    Task<IEnumerable<ContatoDto>> ObterPorDDDAsync(EnumDDD ddd);
    Task<ContatoDto> CriarContatoAsync(CriarContatoCommand dto);
    Task<ContatoDto> AtualizarContatoAsync(int id, AtualizarContatoCommand dto);
    Task ExcluirContatoAsync(int id);
}
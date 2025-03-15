using Fase03.Application.Commands;

namespace Fase03.Application.Interfaces;

public interface IContatosAppService
{
    Task<string> CriarContatoAsync(CriarContatoCommand dto);
    Task<string> AtualizarContatoAsync(int id, AtualizarContatoCommand dto);
    Task ExcluirContatoAsync(int id);
}
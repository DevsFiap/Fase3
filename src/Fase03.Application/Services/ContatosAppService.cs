using AutoMapper;
using Fase03.Application.Commands;
using Fase03.Application.Dto;
using Fase03.Application.Interfaces;
using Fase03.Application.Utils;
using Fase03.Domain.Entities;
using Fase03.Domain.Enums;
using Fase03.Domain.Interfaces.Services;

namespace Fase03.Application.Services;

public class ContatosAppService : IContatosAppService
{
    private readonly IContatoDomainService _contatoDomainService;
    private readonly IMapper _mapper;

    public ContatosAppService(IContatoDomainService contatoDomainService, IMapper mapper)
    {
        _contatoDomainService = contatoDomainService;
        _mapper = mapper;
    }

    public async Task<List<ContatoDto>> GetContatos()
    {
        var contatos = await _contatoDomainService.BuscarContatos();
        return _mapper.Map<List<ContatoDto>>(contatos);
    }

    public async Task<IEnumerable<ContatoDto>> ObterPorDDDAsync(EnumDDD ddd)
    {
        var contatos = await _contatoDomainService.GetByDDDAsync(ddd);
        return _mapper.Map<IEnumerable<ContatoDto>>(contatos);
    }

    public async Task<ContatoDto> CriarContatoAsync(CriarContatoCommand dto)
    {
        try
        {
            if (dto.Telefone.Length < 10)
                throw new ArgumentException("O telefone deve ter pelo menos 10 dígitos (DDD + número).", nameof(dto.Telefone));

            // Extrair DDD e Número do Telefone
            var ddd = dto.Telefone.Substring(0, 2);
            var numeroTelefone = dto.Telefone.Substring(2);

            // Formatar o telefone
            var telefoneFormatado = TelefoneFormatter.FormatarTelefone(dto.Telefone);

            var contato = new Contato
            {
                Nome = dto.Nome,
                Telefone = telefoneFormatado,
                Email = dto.Email,
                DDDTelefone = (EnumDDD)int.Parse(ddd), // Armazenar DDD corretamente
                DataCriacao = DateTime.Now // Atribuir a data de criação
            };

            // Criar o contato no domínio
            await _contatoDomainService.CreateContatoAsync(contato);

            // Mapeie o contato criado para o ContatoDto
            var result = _mapper.Map<ContatoDto>(contato);
            return result;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Erro ao criar contato.", ex);
        }
    }

    public async Task<ContatoDto> AtualizarContatoAsync(int id, AtualizarContatoCommand dto)
    {
        try
        {
            var contatoExistente = await _contatoDomainService.GetByIdAsync(id);
            if (contatoExistente == null)
                throw new KeyNotFoundException("Contato não encontrado.");

            // Verifique se o telefone fornecido é válido
            if (!string.IsNullOrWhiteSpace(dto.Telefone))
            {
                // Verifique se o telefone tem o tamanho correto
                if (dto.Telefone.Length < 10)
                    throw new ArgumentException("O telefone deve ter pelo menos 10 dígitos (DDD + número).", nameof(dto.Telefone));

                // Extrair DDD e Número do Telefone
                var ddd = dto.Telefone.Substring(0, 2);
                var numeroTelefone = dto.Telefone.Substring(2);

                // Formatar o telefone
                contatoExistente.Telefone = TelefoneFormatter.FormatarTelefone(dto.Telefone); // Aplica a formatação
                contatoExistente.DDDTelefone = (EnumDDD)int.Parse(ddd); // Armazena o DDD corretamente
            }

            // Atualiza os demais campos
            contatoExistente.Nome = dto.Nome;
            contatoExistente.Email = dto.Email;

            await _contatoDomainService.UpdateContatoAsync(id, contatoExistente);

            return _mapper.Map<ContatoDto>(contatoExistente);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Erro ao atualizar contato: " + ex.Message);
        }
    }

    public async Task ExcluirContatoAsync(int id)
    {
        try
        {
            var contatoExistente = await _contatoDomainService.GetByIdAsync(id);
            if (contatoExistente == null)
                throw new KeyNotFoundException("Contato não encontrado.");

            await _contatoDomainService.DeleteContatoAsync(id);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Erro ao excluir contato: " + ex.Message);
        }
    }
}
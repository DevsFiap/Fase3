using AutoMapper;
using Fase03.Application.Commands;
using Fase03.Application.Interfaces;
using Fase03.Domain.Interfaces.Messages;
using Fase03.Domain.Models;
using Fase03.Infra.Message.ValueObjects;
using System.Text.Json;

namespace Fase03.Application.Services;

public class ContatosAppService : IContatosAppService
{
    private readonly IMessageQueueProducer _messageQueueProducer;

    public ContatosAppService(IMessageQueueProducer messageQueueProducer)
    {
        _messageQueueProducer = messageQueueProducer;
    }

    public async Task<string> CriarContatoAsync(CriarContatoCommand dto)
    {
        try
        {
            var conteudoMensagem = new ContatosMessageVO
            {
                Nome = dto.Nome,
                Telefone = dto.Telefone,
                Email = dto.Email
            };

            var mensagem = new MessageQueueModel
            {
                Conteudo = JsonSerializer.Serialize(conteudoMensagem),
                Tipo = TipoMensagem.INSERIR_CONTATO
            };

            // Criar o contato e enviar pra fila de contato
             _messageQueueProducer.Create(mensagem);

            return "A mensagem para criação de contato foi enviada";
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Erro ao criar contato.", ex);
        }
    }

    public async Task<string> AtualizarContatoAsync(int id, AtualizarContatoCommand dto)
    {
        try
        {
            var conteudoMensagem = new ContatosMessageVO
            {
                Id = id,
                Nome = dto.Nome,
                Telefone = dto.Telefone,
                Email = dto.Email
            };

            var mensagem = new MessageQueueModel
            {
                Conteudo = JsonSerializer.Serialize(conteudoMensagem),
                Tipo = TipoMensagem.ATUALIZAR_CONTATO
            };

            // Criar o contato no domínio
            _messageQueueProducer.Create(mensagem);

            return "A mensagem para a atualização de contato foi enviada";
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
            var conteudoMensagem = new ContatosMessageVO
            {
                Id = id
            };

            var mensagem = new MessageQueueModel
            {
                Conteudo = JsonSerializer.Serialize(conteudoMensagem),
                Tipo = TipoMensagem.DELETAR_CONTATO
            };

            // Criar o contato no domínio
            _messageQueueProducer.Create(mensagem);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Erro ao excluir contato: " + ex.Message);
        }
    }
}
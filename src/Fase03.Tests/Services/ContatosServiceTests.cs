using AutoMapper;
using FluentAssertions;
using Moq;
using Fase03.Application.Dto;
using Fase03.Application.Services;
using Fase03.Domain.Entities;
using Fase03.Domain.Enums;
using Fase03.Domain.Interfaces.Services;
using TechChallengeFase01.Tests.Builders;
using Fase03.Domain.Interfaces.Messages;
using Fase03.Domain.Models;
using System.Text.Json;

namespace TechChallengeFase01.Tests.Services
{
    public class ContatosServiceTests
    {
        private readonly Mock<IMessageQueueProducer> _messageQueueProducerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ContatosAppService _contatosService;

        public ContatosServiceTests()
        {
            _messageQueueProducerMock = new Mock<IMessageQueueProducer>();
            _mapperMock = new Mock<IMapper>();
            _contatosService = new ContatosAppService(_messageQueueProducerMock.Object, _mapperMock.Object);
        }

        [Fact(DisplayName = "Criar contato com sucesso")]
        public async Task CriarContatoAsync_Should_Return_Contact_Dto_When_Contact_Is_Created_Successfully()
        {
            // Arrange
            var criarContatoDto = new CriarContatoDtoBuilder().Build();

            var contato = new ContatoBuilder().Build();

            var contatoDto = new ContatoDtoBuilder().Build();

            var contatoMessage = "A mensagem para criação de contato foi enviada";

            var messageQueueModel = new MessageQueueModel
            {
                Conteudo = JsonSerializer.Serialize(contatoDto),
                Tipo = TipoMensagem.INSERIR_CONTATO
            };

            _messageQueueProducerMock
                .Setup(service => service.Create(It.IsAny<MessageQueueModel>()));

            // Act
            var result = await _contatosService.CriarContatoAsync(criarContatoDto);

            // Assert
            result.Should().BeEquivalentTo(contatoMessage);
        }

        [Fact(DisplayName = "Atualizar contato com sucesso")]
        public async Task AtualizarContatoAsync_Should_Return_ContactDto_When_Update_Is_Successful()
        {
            // Arrange
            var contatoId = 1;
            var atualizarContatoDto = new AtualizarContatoDtoBuilder().Build();

            var contatoExistente = new ContatoBuilder().Build();

            var contatoDto = new ContatoDtoBuilder().Build();

            var contatoMessage = "A mensagem para a atualização de contato foi enviada";

            var messageQueueModel = new MessageQueueModel
            {
                Conteudo = JsonSerializer.Serialize(contatoDto),
                Tipo = TipoMensagem.ATUALIZAR_CONTATO
            };

            _messageQueueProducerMock
                .Setup(service => service.Create(It.IsAny<MessageQueueModel>()));

            // Act
            var result = await _contatosService.AtualizarContatoAsync(contatoId, atualizarContatoDto);

            // Assert
            result.Should().BeEquivalentTo(contatoMessage);
        }

      /*  [Fact(DisplayName = "Excluir contato com sucesso")]
        public async Task ExcluirContatoAsync_Should_Delete_Contact_When_Contact_Exists()
        {
            // Arrange
            var contatoId = 1;
            var contatoExistente = new ContatoBuilder().Build();

            var messageQueueModel = new MessageQueueModel
            {
                Conteudo = JsonSerializer.Serialize(contatoId),
                Tipo = TipoMensagem.DELETAR_CONTATO
            };

            _messageQueueProducerMock
                .Setup(service => service.Create(It.IsAny<MessageQueueModel>()));

            // Act
            await _contatosService.ExcluirContatoAsync(contatoId);

            // Assert
            _messageQueueProducerMock.Verify(service => service.Create(messageQueueModel), Times.Once);
        } */
    }
}

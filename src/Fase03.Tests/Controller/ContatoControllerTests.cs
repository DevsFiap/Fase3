using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Fase03.Api.Controllers;
using Fase03.Application.Dto;
using Fase03.Application.Interfaces;
using Fase03.Domain.Enums;
using TechChallengeFase01.Tests.Builders;

namespace TechChallengeFase01.Tests.Controller
{
    public class ContatoControllerTests
    {
        private ContatosController _controller;
        private Mock<IContatosAppService> _contatosServiceMock;
        public ContatoControllerTests()
        {
            _contatosServiceMock = new Mock<IContatosAppService>();
            _controller = new ContatosController(_contatosServiceMock.Object);
        }

        [Fact(DisplayName = "Criar contato com sucesso")]
        public async Task CriarContato_Should_Return_Created_When_Contact_Is_Valid()
        {
            // Arrange
            var criarContatoDto = new CriarContatoDtoBuilder().Build();

            var mensagemContatoCriado = "A mensagem para criação de contato foi enviada";

            _contatosServiceMock
                .Setup(service => service.CriarContatoAsync(criarContatoDto))
                .ReturnsAsync(mensagemContatoCriado);

            // Act
            var result = await _controller.CriarContato(criarContatoDto);

            // Assert
            var createdResult = result.Should().BeOfType<ObjectResult>().Subject;
            createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
            createdResult.Value.Should().BeEquivalentTo(mensagemContatoCriado);
        }

        [Fact(DisplayName = "Atualizar contato com sucesso")]
        public async Task AtualizarContato_Should_Return_Ok_When_Contact_Is_Updated_Successfully()
        {
            // Arrange
            var contatoId = 1;
            var atualizarContatoDto = new AtualizarContatoDtoBuilder().Build();

            var mensagemContatoAtualizado = "A mensagem para a atualização de contato foi enviada";

            _contatosServiceMock
                .Setup(service => service.AtualizarContatoAsync(contatoId, atualizarContatoDto))
                .ReturnsAsync(mensagemContatoAtualizado);

            // Act
            var result = await _controller.AtualizarContato(contatoId, atualizarContatoDto);

            // Assert
            var okResult = result.Should().BeOfType<ObjectResult>().Subject;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeEquivalentTo(mensagemContatoAtualizado);
        }

        [Fact(DisplayName = "Deletar contato com sucesso")]
        public async Task DeletarContato_Should_Return_Ok_When_Contact_Is_Deleted_Successfully()
        {
            // Arrange
            var contatoId = 1;

            _contatosServiceMock
                .Setup(service => service.ExcluirContatoAsync(contatoId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeletarContato(contatoId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().Be("Contato excluído com sucesso");
        }
    }
}

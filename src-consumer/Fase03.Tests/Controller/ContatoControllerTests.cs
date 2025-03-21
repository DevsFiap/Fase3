using FluentAssertions;
using Moq;
using Fase03.Application.Dto;
using Fase03.Application.Interfaces;
using Fase03.Domain.Enums;
using TechChallengeFase01.Tests.Builders;

namespace TechChallengeFase01.Tests.Controller
{
    public class ContatoControllerTests
    {
        private Mock<IContatosAppService> _contatosServiceMock;
        public ContatoControllerTests()
        {
            _contatosServiceMock = new Mock<IContatosAppService>();
        }
        /*
        [Fact(DisplayName = "Criar contato com sucesso")]
        public async Task CriarContato_Should_Return_Created_When_Contact_Is_Valid()
        {
            // Arrange
            var criarContatoDto = new CriarContatoDtoBuilder().Build();

            var contatoCriado = new ContatoDtoBuilder().Build();

            _contatosServiceMock
                .Setup(service => service.CriarContatoAsync(criarContatoDto))
                .ReturnsAsync(contatoCriado);

            // Act
            var result = await _controller.CriarContato(criarContatoDto);

            // Assert
            var createdResult = result.Should().BeOfType<ObjectResult>().Subject;
            createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
            createdResult.Value.Should().BeEquivalentTo(contatoCriado);
        }

        [Fact(DisplayName = "Criar contato com retorno BadRequest")]
        public async Task CriarContato_Should_Return_Bad_Request_When_Throws_Exception()
        {
            // Arrange
            var criarContatoDto = new CriarContatoDtoBuilder().Build();

            _contatosServiceMock.Setup(x => x.CriarContatoAsync(criarContatoDto)).Throws<Exception>();

            // Act
            var result = await _controller.CriarContato(criarContatoDto);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact(DisplayName = "Atualizar contato com sucesso")]
        public async Task AtualizarContato_Should_Return_Ok_When_Contact_Is_Updated_Successfully()
        {
            // Arrange
            var contatoId = 1;
            var atualizarContatoDto = new AtualizarContatoDtoBuilder().Build();

            var contatoAtualizado = new ContatoDtoBuilder().Build();

            _contatosServiceMock
                .Setup(service => service.AtualizarContatoAsync(contatoId, atualizarContatoDto))
                .ReturnsAsync(contatoAtualizado);

            // Act
            var result = await _controller.AtualizarContato(contatoId, atualizarContatoDto);

            // Assert
            var okResult = result.Should().BeOfType<ObjectResult>().Subject;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeEquivalentTo(contatoAtualizado);
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

        */
    }
}

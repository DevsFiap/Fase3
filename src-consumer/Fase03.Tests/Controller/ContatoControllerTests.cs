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

        [Fact(DisplayName = "Buscar contatos com sucesso")]
        public async Task GetContacts_Should_Return_Contacts_With_Ok_Status()
        {
            //Arrange
            var contacts = new List<ContatoDto>()
            {
                 new ContatoDtoBuilder().Build()
            };

            _contatosServiceMock.Setup(x => x.GetContatos()).ReturnsAsync(contacts);

            //Act
            var result = await _controller.BuscarContatos();
            var okResult = result as OkObjectResult;
            var contactsResult = okResult?.Value as List<ContatoDto>;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(contactsResult);
            contactsResult.Should().BeEquivalentTo(contacts);
        }

        [Fact(DisplayName = "Buscar contatos com retorno sem conteúdo")]
        public async Task GetContacts_Should_Return_No_Contacts_With_No_Content_Status()
        {
            //Arrange
            _contatosServiceMock.Setup(x => x.GetContatos()).ReturnsAsync((List<ContatoDto>)null);

            //Act
            var result = await _controller.BuscarContatos();
            var noContentResult = result as NoContentResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact(DisplayName = "Buscar contatos com exceção")]
        public async Task GetContacts_Should_Return_Bad_Request_When_Throws_Exception()
        {
            //Arrange
            _contatosServiceMock.Setup(x => x.GetContatos()).Throws<Exception>();

            //Act
            var result = await _controller.BuscarContatos();
            var badRequestResult = result as BadRequestObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact(DisplayName = "Buscar contatos por DDD com sucesso")]
        public async Task GetByDDD_Should_Return_Ok_When_Contacts_Exist()
        {
            // Arrange
            var ddd = EnumDDD.Recife_PE;
            var contacts = new List<ContatoDto>()
            {
                 new ContatoDtoBuilder().Build()
            };

            _contatosServiceMock
                .Setup(service => service.ObterPorDDDAsync(ddd))
                .ReturnsAsync(contacts);

            // Act
            var result = await _controller.BuscarPorDDD(ddd);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeEquivalentTo(contacts);
        }

        [Fact(DisplayName = "Buscar contatos por DDD com retorno NotFound")]
        public async Task GetByDDD_Should_Return_Not_Found_When_No_Contacts_Exist()
        {
            // Arrange
            var ddd = EnumDDD.Recife_PE;

            _contatosServiceMock
                .Setup(service => service.ObterPorDDDAsync(ddd))
                .ReturnsAsync((List<ContatoDto>)null); 

            // Act
            var result = await _controller.BuscarPorDDD(ddd);

            // Assert
            var notFoundResult = result.Should().BeOfType<NoContentResult>().Subject;
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

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
    }
}

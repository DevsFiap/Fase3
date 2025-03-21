using AutoMapper;
using FluentAssertions;
using Moq;
using Fase03.Application.Dto;
using Fase03.Application.Services;
using Fase03.Domain.Entities;
using Fase03.Domain.Enums;
using Fase03.Domain.Interfaces.Services;
using TechChallengeFase01.Tests.Builders;

namespace TechChallengeFase01.Tests.Services
{
    public class ContatosServiceTests
    {
        private readonly Mock<IContatoDomainService> _contatoDomainServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ContatosAppService _contatosService;

        public ContatosServiceTests()
        {
            _contatoDomainServiceMock = new Mock<IContatoDomainService>();
            _mapperMock = new Mock<IMapper>();
            _contatosService = new ContatosAppService(_contatoDomainServiceMock.Object, _mapperMock.Object);
        }

        [Fact(DisplayName = "Obter contatos com sucesso")]
        public async Task GetContatos_Should_Return_Contacts_When_Contacts_Exist()
        {
            // Arrange
            var contatos = new List<Contato> { new ContatoBuilder().Build() };
            var contatoDtos = new List<ContatoDto> { new ContatoDtoBuilder().Build() };

            _contatoDomainServiceMock.Setup(repo => repo.BuscarContatos()).ReturnsAsync(contatos);
            _mapperMock.Setup(mapper => mapper.Map<List<ContatoDto>>(contatos)).Returns(contatoDtos);

            // Act
            var result = await _contatosService.GetContatos();

            // Assert
            result.Should().BeEquivalentTo(contatoDtos);

            _contatoDomainServiceMock.Verify(repo => repo.BuscarContatos(), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<List<ContatoDto>>(contatos), Times.Once);
        }

        [Fact(DisplayName = "Obter contatos com retorno vazio quando nao encontrar")]
        public async Task GetContatos_Should_Return_Empty_List_When_Contacts_Dont_Exist()
        {
            // Arrange
            var emptyContatos = new List<Contato>();
            var emptyContatoDtos = new List<ContatoDto>();

            _contatoDomainServiceMock.Setup(repo => repo.BuscarContatos()).ReturnsAsync(emptyContatos);
            _mapperMock.Setup(mapper => mapper.Map<List<ContatoDto>>(emptyContatos)).Returns(emptyContatoDtos);

            // Act
            var result = await _contatosService.GetContatos();

            // Assert
            result.Should().BeEmpty();

            _contatoDomainServiceMock.Verify(repo => repo.BuscarContatos(), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<List<ContatoDto>>(emptyContatos), Times.Once);
        }

        [Fact(DisplayName = "Obter contatos por DDD com sucesso")]
        public async Task ObterPorDDDAsync_Should_Return_Contacts_When_Contacts_Exist()
        {
            // Arrange
            var ddd = EnumDDD.Recife_PE;

            var contatos = new List<Contato> { new ContatoBuilder().Build() };
            var contatoDtos = new List<ContatoDto> { new ContatoDtoBuilder().Build() };

            _contatoDomainServiceMock.Setup(repo => repo.GetByDDDAsync(ddd)).ReturnsAsync(contatos);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<ContatoDto>>(contatos)).Returns(contatoDtos);

            // Act
            var result = await _contatosService.ObterPorDDDAsync(ddd);

            // Assert
            result.Should().BeEquivalentTo(contatoDtos);

            _contatoDomainServiceMock.Verify(repo => repo.GetByDDDAsync(ddd), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<IEnumerable<ContatoDto>>(contatos), Times.Once);
        }

        [Fact(DisplayName = "Obter contatos por DDD com retorno vazio quando nao encontrar")]
        public async Task ObterPorDDDAsync_Should_Return_Empty_List_When_Contacts_Dont_Exist()
        {
            // Arrange
            var ddd = EnumDDD.Recife_PE;

            var emptyContatos = new List<Contato>();
            var emptyContatoDtos = new List<ContatoDto>();

            _contatoDomainServiceMock.Setup(repo => repo.GetByDDDAsync(ddd)).ReturnsAsync(emptyContatos);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<ContatoDto>>(emptyContatos)).Returns(emptyContatoDtos);

            // Act
            var result = await _contatosService.ObterPorDDDAsync(ddd);

            // Assert
            result.Should().BeEmpty();

            _contatoDomainServiceMock.Verify(repo => repo.GetByDDDAsync(ddd), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<IEnumerable<ContatoDto>>(emptyContatos), Times.Once);
        }

        [Fact(DisplayName = "Criar contato com sucesso")]
        public async Task CriarContatoAsync_Should_Return_Contact_Dto_When_Contact_Is_Created_Successfully()
        {
            // Arrange
            var criarContatoDto = new CriarContatoDtoBuilder().Build();

            var contato = new ContatoBuilder().Build();

            var contatoDto = new ContatoDtoBuilder().Build();

            _contatoDomainServiceMock
                .Setup(service => service.CreateContatoAsync(It.IsAny<Contato>()))
                .Returns(Task.CompletedTask);

            _mapperMock.Setup(mapper => mapper.Map<ContatoDto>(It.IsAny<Contato>())).Returns(contatoDto);

            // Act
            var result = await _contatosService.CriarContatoAsync(criarContatoDto);

            // Assert
            result.Should().BeEquivalentTo(contatoDto);
        }

        [Fact(DisplayName = "Criar contato - exceção quando telefone invalido")]
        public async Task CriarContatoAsync_Should_Throw_ApplicationException_When_Telefone_Is_Invalid()
        {
            // Arrange
            var criarContatoDto = new CriarContatoDtoBuilder().WithInvalidTelefone().Build();

            // Act
            Func<Task> action = async () => await _contatosService.CriarContatoAsync(criarContatoDto);

            // Assert
            await action.Should().ThrowAsync<ApplicationException>()
                .Where(x => x.InnerException != null
                && x.InnerException.Message.Contains("O telefone deve ter pelo menos 10 dígitos (DDD + número)."));
        }

        [Fact(DisplayName = "Criar contato - exceção quando erro inesperado")]
        public async Task CriarContatoAsync_Should_Throw_ApplicationException_When_Unexpected_Error()
        {
            // Arrange
            var criarContatoDto = new CriarContatoDtoBuilder().Build();

            _contatoDomainServiceMock
                .Setup(service => service.CreateContatoAsync(It.IsAny<Contato>()))
                .ThrowsAsync(new Exception("Error"));

            // Act
            Func<Task> action = async () => await _contatosService.CriarContatoAsync(criarContatoDto);

            // Assert
            await action.Should().ThrowAsync<ApplicationException>()
                .WithMessage("Erro ao criar contato.");
        }

        [Fact(DisplayName = "Atualizar contato com sucesso")]
        public async Task AtualizarContatoAsync_Should_Return_ContactDto_When_Update_Is_Successful()
        {
            // Arrange
            var contatoId = 1;
            var atualizarContatoDto = new AtualizarContatoDtoBuilder().Build();

            var contatoExistente = new ContatoBuilder().Build();

            var contatoDto = new ContatoDtoBuilder().Build();

            _contatoDomainServiceMock
                .Setup(service => service.GetByIdAsync(contatoId))
                .ReturnsAsync(contatoExistente);

            _contatoDomainServiceMock
                .Setup(service => service.UpdateContatoAsync(contatoId, contatoExistente))
                .Returns(Task.CompletedTask); 

            _mapperMock
                .Setup(mapper => mapper.Map<ContatoDto>(contatoExistente))
                .Returns(contatoDto);

            // Act
            var result = await _contatosService.AtualizarContatoAsync(contatoId, atualizarContatoDto);

            // Assert
            result.Should().BeEquivalentTo(contatoDto);
        }

        [Fact(DisplayName = "Atualizar contato - exceção quando contato não existir")]
        public async Task AtualizarContatoAsync_Should_Throw_ApplicationException_When_Contact_Does_Not_Exist()
        {
            // Arrange
            var contatoId = 1;
            var atualizarContatoDto = new AtualizarContatoDtoBuilder().Build();

            _contatoDomainServiceMock
                .Setup(service => service.GetByIdAsync(contatoId))
                .ReturnsAsync((Contato)null); 

            // Act
            Func<Task> action = async () => await _contatosService.AtualizarContatoAsync(contatoId, atualizarContatoDto);

            // Assert
            await action.Should().ThrowAsync<ApplicationException>()
                .WithMessage("Erro ao atualizar contato: Contato não encontrado.");
        }

        [Fact(DisplayName = "Atualizar contato - exceção quando contato telefone for inválido")]
        public async Task AtualizarContatoAsync_Should_Throw_ApplicationException_When_Telefone_Is_Invalid()
        {
            // Arrange
            var contatoId = 1;
            var atualizarContatoDto = new AtualizarContatoDtoBuilder().WithInvalidTelefone().Build();

            var contatoExistente = new ContatoBuilder().Build();

            _contatoDomainServiceMock
                .Setup(service => service.GetByIdAsync(contatoId))
                .ReturnsAsync(contatoExistente);

            // Act
            Func<Task> action = async () => await _contatosService.AtualizarContatoAsync(contatoId, atualizarContatoDto);

            // Assert
            await action.Should().ThrowAsync<ApplicationException>()
                .WithMessage("Erro ao atualizar contato: O telefone deve ter pelo menos 10 dígitos (DDD + número).*");
        }

        [Fact(DisplayName = "Atualizar contato - exceção quando houver erro inesperado")]
        public async Task AtualizarContatoAsync_Should_Throw_ApplicationException_When_Unexpected_Error()
        {
            // Arrange
            var contatoId = 1;
            var atualizarContatoDto = new AtualizarContatoDtoBuilder().Build();

            var contatoExistente = new ContatoBuilder().Build();

            _contatoDomainServiceMock
                .Setup(service => service.GetByIdAsync(contatoId))
                .ReturnsAsync(contatoExistente);

            _contatoDomainServiceMock
                .Setup(service => service.UpdateContatoAsync(contatoId, contatoExistente))
                .ThrowsAsync(new Exception("Erro inesperado"));

            // Act
            Func<Task> action = async () => await _contatosService.AtualizarContatoAsync(contatoId, atualizarContatoDto);

            // Assert
            await action.Should().ThrowAsync<ApplicationException>()
                .WithMessage("Erro ao atualizar contato: Erro inesperado");
        }

        [Fact(DisplayName = "Excluir contato com sucesso")]
        public async Task ExcluirContatoAsync_Should_Delete_Contact_When_Contact_Exists()
        {
            // Arrange
            var contatoId = 1;
            var contatoExistente = new ContatoBuilder().Build();

            _contatoDomainServiceMock
                .Setup(service => service.GetByIdAsync(contatoId))
                .ReturnsAsync(contatoExistente);

            _contatoDomainServiceMock
                .Setup(service => service.DeleteContatoAsync(contatoId))
                .Returns(Task.CompletedTask);

            // Act
            await _contatosService.ExcluirContatoAsync(contatoId);

            // Assert
            _contatoDomainServiceMock.Verify(service => service.DeleteContatoAsync(contatoId), Times.Once);
        }

        [Fact(DisplayName = "Excluir contato - exceção quando contato não existir")]
        public async Task ExcluirContatoAsync_Should_Throw_ApplicationException_When_Contact_Does_Not_Exist()
        {
            // Arrange
            var contatoId = 1;

            _contatoDomainServiceMock
                .Setup(service => service.GetByIdAsync(contatoId))
                .ReturnsAsync((Contato)null);

            // Act
            Func<Task> action = async () => await _contatosService.ExcluirContatoAsync(contatoId);

            // Assert
            await action.Should().ThrowAsync<ApplicationException>()
                .WithMessage("Erro ao excluir contato: Contato não encontrado.");
        }

        [Fact(DisplayName = "Excluir contato - exceção quando houver erro inesperado")]
        public async Task ExcluirContatoAsync_Should_Throw_ApplicationException_When_Unexpected_Error()
        {
            // Arrange
            var contatoId = 1;
            var contatoExistente = new ContatoBuilder().Build();

            _contatoDomainServiceMock
                .Setup(service => service.GetByIdAsync(contatoId))
                .ReturnsAsync(contatoExistente);

            _contatoDomainServiceMock
                .Setup(service => service.DeleteContatoAsync(contatoId))
                .ThrowsAsync(new Exception("Erro inesperado"));

            // Act
            Func<Task> action = async () => await _contatosService.ExcluirContatoAsync(contatoId);

            // Assert
            await action.Should().ThrowAsync<ApplicationException>()
                .WithMessage("Erro ao excluir contato: Erro inesperado");
        }
    }
}

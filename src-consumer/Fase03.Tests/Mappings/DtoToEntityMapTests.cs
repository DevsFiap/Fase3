using AutoMapper;
using FluentAssertions;
using Fase03.Application.Dto;
using Fase03.Application.Mappings;
using Fase03.Domain.Entities;
using TechChallengeFase01.Tests.Builders;

namespace TechChallengeFase01.Tests.Mappings
{
    public class DtoToEntityMapTests
    {
        private readonly IMapper _mapper;

        public DtoToEntityMapTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoToEntityMap>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact(DisplayName = "Mapear Contato para ContatoDto com sucesso")]
        public void Should_Map_Contact_To_ContactDto()
        {
            // Arrange
            var contato = new ContatoBuilder().Build();
            // Act
            var contatoDto = _mapper.Map<ContatoDto>(contato);

            // Assert
            contatoDto.Should().NotBeNull();
            contatoDto.Nome.Should().Be(contato.Nome);
            contatoDto.Email.Should().Be(contato.Email);
            contatoDto.DataCriacao.Should().Be(contato.DataCriacao);
            contatoDto.DDDTelefone.Should().Be(contato.DDDTelefone);
            contatoDto.NumeroTelefone.Should().Be(contato.Telefone);
        }

        [Fact(DisplayName = "Mapear CriarContatoDto para Contato com sucesso")]
        public void Should_Map_CreateContactDto_To_Contact()
        {
            // Arrange
            var criarContatoDto = new CriarContatoDtoBuilder().Build();

            // Act
            var contato = _mapper.Map<Contato>(criarContatoDto);

            // Assert
            contato.Should().NotBeNull();
            contato.Nome.Should().Be(criarContatoDto.Nome);
            contato.Email.Should().Be(criarContatoDto.Email);
            contato.Telefone.Should().Be(criarContatoDto.Telefone);
        }

        [Fact(DisplayName = "Mapear AtualizarContatoDto para Contato com sucesso")]
        public void Should_Map_UpdateContactDtoDto_To_Contact()
        {
            // Arrange
            var atualizarContatoDto = new AtualizarContatoDtoBuilder().Build();

            // Act
            var contato = _mapper.Map<Contato>(atualizarContatoDto);

            // Assert
            contato.Should().NotBeNull();
            contato.Nome.Should().Be(atualizarContatoDto.Nome);
            contato.Email.Should().Be(atualizarContatoDto.Email);
            contato.Telefone.Should().Be(atualizarContatoDto.Telefone);
        }
    }
}

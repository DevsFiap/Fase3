using Fase03.Domain.Entities;
using Fase03.Domain.Enums;

namespace TechChallengeFase01.Tests.Builders
{
    public class ContatoBuilder
    {
        private Contato _contato;

        public ContatoBuilder()
        {
            _contato = new Contato
            {
                Id = 1,
                Nome = "Nome",
                Telefone = "(81) 99999-9999",
                Email = "email@email.com",
                DDDTelefone = EnumDDD.Recife_PE,
                DataCriacao = DateTime.UtcNow
            };
        }

        public Contato Build()
        {
            return _contato;
        }
    }
}

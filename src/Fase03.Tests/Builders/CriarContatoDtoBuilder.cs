using Fase03.Application.Commands;

namespace TechChallengeFase01.Tests.Builders
{
    public class CriarContatoDtoBuilder
    {
        private string _telefone = "81999999999";
        private string _nome = "Nome";
        private string _email = "email@email.com";

        public CriarContatoDtoBuilder()
        {
        }

        public CriarContatoDtoBuilder WithInvalidTelefone()
        {
            _telefone = "999";
            return this;
        }

        public CriarContatoCommand Build()
        {
            return new CriarContatoCommand
            {
                Nome = _nome,
                Telefone = _telefone,
                Email = _email
            };
        }
    }
}

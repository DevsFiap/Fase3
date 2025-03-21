using Fase03.Application.Commands;

namespace TechChallengeFase01.Tests.Builders
{
    public class AtualizarContatoDtoBuilder
    {
        private string _telefone = "81999999999";
        private string _nome = "Nome";
        private string _email = "email@email.com";

        public AtualizarContatoDtoBuilder()
        {
        }

        public AtualizarContatoDtoBuilder WithInvalidTelefone()
        {
            _telefone = "999";
            return this;
        }

        public AtualizarContatoCommand Build()
        {
            return new AtualizarContatoCommand
            {
                Nome = _nome,
                Telefone = _telefone,
                Email = _email
            };
        }
    }
}

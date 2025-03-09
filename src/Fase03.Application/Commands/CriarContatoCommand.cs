using System.ComponentModel.DataAnnotations;
using Fase03.Application.Dto;
using Fase03.Application.Utils;
using Fase03.Domain.Entities;
using Fase03.Domain.Enums;
using MediatR;

namespace Fase03.Application.Commands;

public class CriarContatoCommand : IRequest<ContatoDto>
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MinLength(8, ErrorMessage = "O nome deve ter no minimo 8 letras")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [RegularExpression(@"^\d{10,11}$", ErrorMessage = "O telefone deve ter entre 10 e 11 dígitos, sem formatação.")]
    public string Telefone { get; set; }

    [Required(ErrorMessage = "O E-Mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; }

    public string TelefoneFormatado => TelefoneFormatter.FormatarTelefone(Telefone);

    public Contato ConverterParaContato()
    {
        // Separar DDD e número
        var ddd = Telefone.Substring(0, 2);
        var numeroTelefone = Telefone.Substring(2);

        return new Contato
        {
            Nome = Nome,
            Telefone = numeroTelefone, // Armazena apenas o número
            DDDTelefone = (EnumDDD)int.Parse(ddd), // Armazena o DDD
            Email = Email,
            DataCriacao = DateTime.Now
        };
    }
}
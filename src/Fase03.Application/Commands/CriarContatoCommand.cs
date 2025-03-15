using Fase03.Application.Dto;
using MediatR;
using System.ComponentModel.DataAnnotations;

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
}
using Fase03.Application.Dto;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Fase03.Application.Commands;

public class AtualizarContatoCommand : IRequest<ContatoDto>
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [RegularExpression(@"^\d{9,11}$", ErrorMessage = "O telefone deve ter entre 9 e 11 dígitos, sem formatação.")]
    public string Telefone { get; set; }

    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; }
}
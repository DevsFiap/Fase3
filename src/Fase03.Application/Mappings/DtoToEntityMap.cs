using AutoMapper;
using Fase03.Application.Commands;
using Fase03.Application.Dto;
using Fase03.Domain.Entities;

namespace Fase03.Application.Mappings;

public class DtoToEntityMap : Profile
{
    public DtoToEntityMap()
    {
        CreateMap<CriarContatoCommand, Contato>().ReverseMap();
        CreateMap<AtualizarContatoCommand, Contato>().ReverseMap();
    }
}
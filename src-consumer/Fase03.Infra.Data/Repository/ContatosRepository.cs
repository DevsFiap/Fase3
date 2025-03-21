using Microsoft.EntityFrameworkCore;
using Fase03.Domain.Entities;
using Fase03.Domain.Enums;
using Fase03.Domain.Interfaces.Repositories;
using Fase03.Infra.Data.Context;

namespace Fase03.Infra.Data.Repository;

public class ContatoRepository : BaseRepository<Contato>, IContatosRepository
{
    private readonly AppDbContext _context;

    public ContatoRepository(AppDbContext context) : base(context)
        => _context = context;

    public async Task<IEnumerable<Contato>> GetByDDDAsync(EnumDDD ddd)
    {
        if (!Enum.IsDefined(typeof(EnumDDD), ddd))
            throw new ArgumentException("DDD inválido.", nameof(ddd));

        string dddString = ((int)ddd).ToString();

        return await _context.Set<Contato>()
            .Where(c => c.Telefone.StartsWith($"({dddString})"))
            .ToListAsync();
    }
}
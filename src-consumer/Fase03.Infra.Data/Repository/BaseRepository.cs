using Microsoft.EntityFrameworkCore;
using Fase03.Domain.Core;
using Fase03.Domain.Exceptions;
using Fase03.Infra.Data.Context;

namespace Fase03.Infra.Data.Repository;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class
{
    private readonly AppDbContext _context;

    protected BaseRepository(AppDbContext context)
        => _context = context;

    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        try
        {
            return await _context.Set<TEntity>().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Erro ao obter todos os registros.", ex);
        }
    }

    public virtual async Task<TEntity> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Erro ao buscar a entidade com ID {id}.", ex);
        }
    }

    public virtual async Task CreateAsync(TEntity entity)
    {
        try
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException dbEx)
        {
            throw new RepositoryException("Erro ao criar a entidade. Verifique as restrições e integridade dos dados.", dbEx);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Erro inesperado ao criar a entidade.", ex);
        }
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException dbConEx)
        {
            throw new RepositoryException("Falha de concorrência ao atualizar a entidade.", dbConEx);
        }
        catch (DbUpdateException dbEx)
        {
            throw new RepositoryException("Erro ao atualizar a entidade. Verifique as restrições de integridade.", dbEx);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Erro inesperado ao atualizar a entidade.", ex);
        }
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException dbEx)
        {
            throw new RepositoryException("Erro ao deletar a entidade. Verifique se a entidade está relacionada a outros dados.", dbEx);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Erro inesperado ao deletar a entidade.", ex);
        }
    }

    public void Dispose()
        => _context.Dispose();
}
using DevInsight.Core.Entities;
using DevInsight.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevInsight.Infrastructure.Data;

public class ProjetoRepository : Repository<ProjetoConsultoria>, IProjetoRepository
{
    public ProjetoRepository(DbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProjetoConsultoria>> GetByUsuarioIdAsync(Guid usuarioId)
    {
        return await _dbSet
            .Where(p => p.CriadoPorId == usuarioId)
            .ToListAsync();
    }
}
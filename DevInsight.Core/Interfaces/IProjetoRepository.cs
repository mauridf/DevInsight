using DevInsight.Core.Entities;

namespace DevInsight.Core.Interfaces;

public interface IProjetoRepository : IRepository<ProjetoConsultoria>
{
    Task<IEnumerable<ProjetoConsultoria>> GetByUsuarioIdAsync(Guid usuarioId);
}

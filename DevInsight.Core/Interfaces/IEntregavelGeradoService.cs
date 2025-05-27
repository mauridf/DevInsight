using DevInsight.Core.DTOs;

namespace DevInsight.Core.Interfaces.Services;

public interface IEntregavelGeradoService
{
    Task<EntregavelGeradoConsultaDTO> CriarEntregavelAsync(EntregavelGeradoCriacaoDTO entregavelDto, Guid projetoId);
    Task<EntregavelGeradoConsultaDTO> ObterPorIdAsync(Guid id);
    Task<IEnumerable<EntregavelGeradoConsultaDTO>> ListarPorProjetoAsync(Guid projetoId);
    Task<bool> ExcluirEntregavelAsync(Guid id);
    Task<string> GerarUrlDownloadAsync(Guid id);
}
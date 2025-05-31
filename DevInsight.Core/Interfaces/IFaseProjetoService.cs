using DevInsight.Core.DTOs;

namespace DevInsight.Core.Interfaces;

public interface IFaseProjetoService
{
    Task<FaseProjetoConsultaDTO> CriarFaseProjetoAsync(FaseProjetoCriacaoDTO faseDto, Guid projetoId);
    Task<FaseProjetoConsultaDTO> ObterPorIdAsync(Guid id);
    Task<IEnumerable<FaseProjetoConsultaDTO>> ListarPorProjetoAsync(Guid projetoId);
    Task<FaseProjetoConsultaDTO> AtualizarFaseProjetoAsync(Guid id, FaseProjetoAtualizacaoDTO faseDto);
    Task<bool> ExcluirFaseProjetoAsync(Guid id);
}

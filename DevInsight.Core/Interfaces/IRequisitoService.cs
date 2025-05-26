using DevInsight.Core.DTOs;

namespace DevInsight.Core.Interfaces.Services;

public interface IRequisitoService
{
    Task<RequisitoConsultaDTO> CriarRequisitoAsync(RequisitoCriacaoDTO requisitoDto, Guid projetoId);
    Task<RequisitoConsultaDTO> ObterPorIdAsync(Guid id);
    Task<IEnumerable<RequisitoConsultaDTO>> ListarPorProjetoAsync(Guid projetoId);
    Task<RequisitoConsultaDTO> AtualizarRequisitoAsync(Guid id, RequisitoAtualizacaoDTO requisitoDto);
    Task<bool> ExcluirRequisitoAsync(Guid id);
}
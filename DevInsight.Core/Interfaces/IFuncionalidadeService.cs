using DevInsight.Core.DTOs;

namespace DevInsight.Core.Interfaces.Services;

public interface IFuncionalidadeService
{
    Task<FuncionalidadeConsultaDTO> CriarFuncionalidadeAsync(FuncionalidadeCriacaoDTO funcionalidadeDto, Guid projetoId);
    Task<FuncionalidadeConsultaDTO> ObterPorIdAsync(Guid id);
    Task<IEnumerable<FuncionalidadeConsultaDTO>> ListarPorProjetoAsync(Guid projetoId);
    Task<FuncionalidadeConsultaDTO> AtualizarFuncionalidadeAsync(Guid id, FuncionalidadeAtualizacaoDTO funcionalidadeDto);
    Task<bool> ExcluirFuncionalidadeAsync(Guid id);
}
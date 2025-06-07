using DevInsight.Core.DTOs;

namespace DevInsight.Core.Interfaces.Services;

public interface IEntregavelGeradoService
{
    Task<EntregavelGeradoConsultaDTO> CriarEntregavelAsync(EntregavelGeradoCriacaoDTO entregavelDto, Guid projetoId);
    Task<EntregavelGeradoConsultaDTO> ObterPorIdAsync(Guid id);
    Task<IEnumerable<EntregavelGeradoConsultaDTO>> ListarPorProjetoAsync(Guid projetoId);
    Task<bool> ExcluirEntregavelAsync(Guid id);
    Task<string> GerarUrlDownloadAsync(Guid id);
    Task<DadosRelatorioConsultoria> ObterDadosRelatorioConsultoriaAsync(Guid projetoId);
    Task<byte[]> GeneratePdfFromHtmlAsync(string htmlContent);
    Task<byte[]> GenerateDocxFromHtmlAsync(string htmlContent);
    Task<string> GenerateMarkdownFromHtmlAsync(string htmlContent);
    Task<byte[]> GenerateExcelFromDataAsync<T>(IEnumerable<T> data);
}
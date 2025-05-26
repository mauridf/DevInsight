using DevInsight.Core.DTOs;

namespace DevInsight.Core.Interfaces.Services;

public interface IDocumentoLinkService
{
    Task<DocumentoLinkConsultaDTO> CriarDocumentoLinkAsync(DocumentoLinkCriacaoDTO documentoLinkDto, Guid projetoId);
    Task<DocumentoLinkConsultaDTO> ObterPorIdAsync(Guid id);
    Task<IEnumerable<DocumentoLinkConsultaDTO>> ListarPorProjetoAsync(Guid projetoId);
    Task<DocumentoLinkConsultaDTO> AtualizarDocumentoLinkAsync(Guid id, DocumentoLinkAtualizacaoDTO documentoLinkDto);
    Task<bool> ExcluirDocumentoLinkAsync(Guid id);
}
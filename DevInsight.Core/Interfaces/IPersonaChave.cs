using DevInsight.Core.DTOs;

namespace DevInsight.Core.Interfaces;

public interface IPersonaChave
{
    Task<PersonaChaveConsultaDTO> CriarPersonaAsync(PersonaChaveCriacaoDTO personaDto, Guid projetoId);
    Task<PersonaChaveConsultaDTO> ObterPorIdAsync(Guid id);
    Task<IEnumerable<PersonaChaveConsultaDTO>> ListarPorProjetoAsync(Guid projetoId);
    Task<PersonaChaveConsultaDTO> AtualizarPersonaAsync(Guid id, PersonaChaveAtualizacaoDTO personaDto);
    Task<bool> ExcluirPersonaAsync(Guid id);
}

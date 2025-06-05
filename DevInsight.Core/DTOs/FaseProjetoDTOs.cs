using DevInsight.Core.Entities;
using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class FaseProjetoCriacaoDTO
{
    public Guid ProjetoId { get; set; }
    public string Fase { get; set; }
    public string Objetivo { get; set; }
    public int DuracaoEstimada { get; set; }
}

public class FaseProjetoAtualizacaoDTO
{
    public Guid ProjetoId { get; set; }
    public string Fase { get; set; }
    public string Objetivo { get; set; }
    public int DuracaoEstimada { get; set; }
}

public class FaseProjetoConsultaDTO
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public string Fase { get; set; }
    public string Objetivo { get; set; }
    public int DuracaoEstimada { get; set; }
    public DateTime CriadoEm { get; set; }
}


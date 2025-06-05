using DevInsight.Core.Entities;

namespace DevInsight.Core.DTOs;

public class FuncionalidadeCriacaoDTO
{
    public Guid ProjetoId { get; set; }
    public string Funcionalidade { get; set; } = null!;
}

public class FuncionalidadeAtualizacaoDTO
{
    public Guid ProjetoId { get; set; }
    public string Funcionalidade { get; set; } = null!;
}

public class FuncionalidadeConsultaDTO
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; }
    public string Funcionalidade { get; set; } = null!;
    public DateTime CriadoEm { get; set; }
}
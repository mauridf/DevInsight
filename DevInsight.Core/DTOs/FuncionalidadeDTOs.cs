namespace DevInsight.Core.DTOs;

public class FuncionalidadeCriacaoDTO
{
    public string Funcionalidade { get; set; } = null!;
}

public class FuncionalidadeAtualizacaoDTO
{
    public string Funcionalidade { get; set; } = null!;
}

public class FuncionalidadeConsultaDTO
{
    public Guid Id { get; set; }
    public string Funcionalidade { get; set; } = null!;
    public DateTime CriadoEm { get; set; }
}
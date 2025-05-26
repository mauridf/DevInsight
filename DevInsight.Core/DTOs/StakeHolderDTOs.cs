namespace DevInsight.Core.DTOs;

public class StakeHolderCriacaoDTO
{
    public string Nome { get; set; } = null!;
    public string Funcao { get; set; } = null!;
}

public class StakeHolderAtualizacaoDTO
{
    public string Nome { get; set; } = null!;
    public string Funcao { get; set; } = null!;
}

public class StakeHolderConsultaDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Funcao { get; set; } = null!;
    public DateTime CriadoEm { get; set; }
}
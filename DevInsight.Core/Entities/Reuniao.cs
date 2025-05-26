using System.ComponentModel.DataAnnotations;

namespace DevInsight.Core.Entities;

public class Reuniao
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public string Titulo { get; set; }
    public DateTime DataHora { get; set; }
    [MaxLength(500)]
    public string Link { get; set; }
    [MaxLength(1000)]
    public string Observacoes { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}

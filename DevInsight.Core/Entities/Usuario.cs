using System.ComponentModel.DataAnnotations;
using DevInsight.Core.Enums;

namespace DevInsight.Core.Entities;

public class Usuario
{
    public Guid Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = null!;
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = null!;
    public string SenhaHash { get; set; } = null!;
    public TipoUsuario TipoUsuario { get; set; }
    public bool EmailConfirmado { get; set; } = false;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public ICollection<ProjetoConsultoria> ProjetosCriados { get; set; } = new List<ProjetoConsultoria>();
}
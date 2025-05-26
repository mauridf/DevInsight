using DevInsight.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DevInsight.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<ProjetoConsultoria> ProjetosConsultoria { get; set; }
    public DbSet<StakeHolder> StakeHolders { get; set; }
    public DbSet<FuncionalidadeDesejada> FuncionalidadesDesejadas { get; set; }
    public DbSet<Requisito> Requisitos { get; set; }
    public DbSet<DocumentoLink> DocumentosLinks { get; set; }
    public DbSet<Reuniao> Reunioes { get; set; }
    public DbSet<TarefaProjeto> TarefasProjeto { get; set; }
    public DbSet<ValidacaoTecnica> ValidacoesTecnicas { get; set; }
    public DbSet<EntregaFinal> EntregasFinais { get; set; }
    public DbSet<SolucaoProposta> SolucoesPropostas { get; set; }
    public DbSet<EntregavelGerado> EntregaveisGerados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração de Usuário
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.TipoUsuario).HasConversion<string>();
            entity.Property(u => u.Nome).HasMaxLength(100).IsRequired();
            entity.Property(u => u.Email).HasMaxLength(100).IsRequired();
            entity.Property(u => u.SenhaHash).IsRequired();
        });

        // Configuração de ProjetoConsultoria
        modelBuilder.Entity<ProjetoConsultoria>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nome).HasMaxLength(200).IsRequired();
            entity.Property(p => p.Cliente).HasMaxLength(200).IsRequired();
            entity.Property(p => p.Status).HasConversion<string>();

            entity.HasOne(p => p.CriadoPor)
                  .WithMany(u => u.ProjetosCriados)
                  .HasForeignKey(p => p.CriadoPorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração para todas as entidades que relacionam com ProjetoConsultoria
        var projetoRelations = new[]
        {
            typeof(StakeHolder),
            typeof(FuncionalidadeDesejada),
            typeof(Requisito),
            typeof(DocumentoLink),
            typeof(Reuniao),
            typeof(TarefaProjeto),
            typeof(ValidacaoTecnica),
            typeof(EntregaFinal),
            typeof(SolucaoProposta),
            typeof(EntregavelGerado)
        };

        foreach (var type in projetoRelations)
        {
            modelBuilder.Entity(type).HasOne("Projeto")
                      .WithMany()
                      .HasForeignKey("ProjetoId")
                      .OnDelete(DeleteBehavior.Cascade);
        }

        // Configurações específicas para algumas entidades
        modelBuilder.Entity<Requisito>(entity =>
        {
            entity.Property(r => r.TipoRequisito).HasConversion<string>();
            entity.Property(r => r.Descricao).HasMaxLength(1000);
        });

        modelBuilder.Entity<DocumentoLink>(entity =>
        {
            entity.Property(d => d.TipoDocumento).HasConversion<string>();
            entity.Property(d => d.Url).HasMaxLength(500);
        });

        modelBuilder.Entity<TarefaProjeto>(entity =>
        {
            entity.Property(t => t.Status).HasConversion<string>();
            entity.Property(t => t.Titulo).HasMaxLength(200);
        });

        modelBuilder.Entity<ValidacaoTecnica>(entity =>
        {
            entity.Property(v => v.Tipo).HasConversion<string>();
        });

        modelBuilder.Entity<EntregaFinal>(entity =>
        {
            entity.Property(e => e.Tipo).HasConversion<string>();
        });

        modelBuilder.Entity<EntregavelGerado>(entity =>
        {
            entity.Property(e => e.Tipo).HasConversion<string>();
            entity.Property(e => e.Formato).HasConversion<string>();
        });

        // Configurar precisão para campos DateTime
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetPrecision(6); // Adiciona precisão de milissegundos
                }
            }
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");

        base.ConfigureConventions(configurationBuilder);
    }

    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter() : base(
            dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
            dateTime => DateOnly.FromDateTime(dateTime))
        { }
    }
}
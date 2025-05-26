using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevInsight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SenhaHash = table.Column<string>(type: "text", nullable: false),
                    TipoUsuario = table.Column<string>(type: "text", nullable: false),
                    EmailConfirmado = table.Column<bool>(type: "boolean", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjetosConsultoria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cliente = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CriadoPorId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosConsultoria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetosConsultoria_Usuarios_CriadoPorId",
                        column: x => x.CriadoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentosLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjetoId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoDocumento = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    ProjetoConsultoriaId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentosLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentosLinks_ProjetosConsultoria_ProjetoConsultoriaId",
                        column: x => x.ProjetoConsultoriaId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentosLinks_ProjetosConsultoria_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntregasFinais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjetoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    UrlEntrega = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    ProjetoConsultoriaId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntregasFinais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntregasFinais_ProjetosConsultoria_ProjetoConsultoriaId",
                        column: x => x.ProjetoConsultoriaId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntregasFinais_ProjetosConsultoria_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntregaveisGerados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjetoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    Formato = table.Column<string>(type: "text", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    ProjetoConsultoriaId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntregaveisGerados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntregaveisGerados_ProjetosConsultoria_ProjetoConsultoriaId",
                        column: x => x.ProjetoConsultoriaId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntregaveisGerados_ProjetosConsultoria_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FuncionalidadesDesejadas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjetoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Funcionalidade = table.Column<string>(type: "text", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    ProjetoConsultoriaId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuncionalidadesDesejadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuncionalidadesDesejadas_ProjetosConsultoria_ProjetoConsult~",
                        column: x => x.ProjetoConsultoriaId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FuncionalidadesDesejadas_ProjetosConsultoria_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requisitos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjetoId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoRequisito = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    ProjetoConsultoriaId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requisitos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requisitos_ProjetosConsultoria_ProjetoConsultoriaId",
                        column: x => x.ProjetoConsultoriaId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requisitos_ProjetosConsultoria_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reunioes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjetoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    DataHora = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    Link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Observacoes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    ProjetoConsultoriaId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reunioes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reunioes_ProjetosConsultoria_ProjetoConsultoriaId",
                        column: x => x.ProjetoConsultoriaId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reunioes_ProjetosConsultoria_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolucoesPropostas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjetoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Resumo = table.Column<string>(type: "text", nullable: false),
                    Arquitetura = table.Column<string>(type: "text", nullable: false),
                    ComponentesSistema = table.Column<string>(type: "text", nullable: false),
                    PontosIntegracao = table.Column<string>(type: "text", nullable: false),
                    Riscos = table.Column<string>(type: "text", nullable: false),
                    RecomendacoesTecnicas = table.Column<string>(type: "text", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    ProjetoConsultoriaId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolucoesPropostas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolucoesPropostas_ProjetosConsultoria_ProjetoConsultoriaId",
                        column: x => x.ProjetoConsultoriaId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SolucoesPropostas_ProjetosConsultoria_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StakeHolders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjetoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Funcao = table.Column<string>(type: "text", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    ProjetoConsultoriaId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StakeHolders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StakeHolders_ProjetosConsultoria_ProjetoConsultoriaId",
                        column: x => x.ProjetoConsultoriaId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StakeHolders_ProjetosConsultoria_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TarefasProjeto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjetoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    DataEntrega = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    ProjetoConsultoriaId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarefasProjeto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TarefasProjeto_ProjetosConsultoria_ProjetoConsultoriaId",
                        column: x => x.ProjetoConsultoriaId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TarefasProjeto_ProjetosConsultoria_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValidacoesTecnicas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjetoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Validado = table.Column<bool>(type: "boolean", nullable: false),
                    Observacao = table.Column<string>(type: "text", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    ProjetoConsultoriaId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidacoesTecnicas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValidacoesTecnicas_ProjetosConsultoria_ProjetoConsultoriaId",
                        column: x => x.ProjetoConsultoriaId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ValidacoesTecnicas_ProjetosConsultoria_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosLinks_ProjetoConsultoriaId",
                table: "DocumentosLinks",
                column: "ProjetoConsultoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosLinks_ProjetoId",
                table: "DocumentosLinks",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntregasFinais_ProjetoConsultoriaId",
                table: "EntregasFinais",
                column: "ProjetoConsultoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_EntregasFinais_ProjetoId",
                table: "EntregasFinais",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntregaveisGerados_ProjetoConsultoriaId",
                table: "EntregaveisGerados",
                column: "ProjetoConsultoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_EntregaveisGerados_ProjetoId",
                table: "EntregaveisGerados",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_FuncionalidadesDesejadas_ProjetoConsultoriaId",
                table: "FuncionalidadesDesejadas",
                column: "ProjetoConsultoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_FuncionalidadesDesejadas_ProjetoId",
                table: "FuncionalidadesDesejadas",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosConsultoria_CriadoPorId",
                table: "ProjetosConsultoria",
                column: "CriadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitos_ProjetoConsultoriaId",
                table: "Requisitos",
                column: "ProjetoConsultoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitos_ProjetoId",
                table: "Requisitos",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reunioes_ProjetoConsultoriaId",
                table: "Reunioes",
                column: "ProjetoConsultoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reunioes_ProjetoId",
                table: "Reunioes",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_SolucoesPropostas_ProjetoConsultoriaId",
                table: "SolucoesPropostas",
                column: "ProjetoConsultoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_SolucoesPropostas_ProjetoId",
                table: "SolucoesPropostas",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_StakeHolders_ProjetoConsultoriaId",
                table: "StakeHolders",
                column: "ProjetoConsultoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_StakeHolders_ProjetoId",
                table: "StakeHolders",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_TarefasProjeto_ProjetoConsultoriaId",
                table: "TarefasProjeto",
                column: "ProjetoConsultoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_TarefasProjeto_ProjetoId",
                table: "TarefasProjeto",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ValidacoesTecnicas_ProjetoConsultoriaId",
                table: "ValidacoesTecnicas",
                column: "ProjetoConsultoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidacoesTecnicas_ProjetoId",
                table: "ValidacoesTecnicas",
                column: "ProjetoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentosLinks");

            migrationBuilder.DropTable(
                name: "EntregasFinais");

            migrationBuilder.DropTable(
                name: "EntregaveisGerados");

            migrationBuilder.DropTable(
                name: "FuncionalidadesDesejadas");

            migrationBuilder.DropTable(
                name: "Requisitos");

            migrationBuilder.DropTable(
                name: "Reunioes");

            migrationBuilder.DropTable(
                name: "SolucoesPropostas");

            migrationBuilder.DropTable(
                name: "StakeHolders");

            migrationBuilder.DropTable(
                name: "TarefasProjeto");

            migrationBuilder.DropTable(
                name: "ValidacoesTecnicas");

            migrationBuilder.DropTable(
                name: "ProjetosConsultoria");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}

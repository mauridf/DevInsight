using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevInsight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NovosCamposEEntidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Observacoes",
                table: "TarefasProjeto",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "SituacaoAtual",
                table: "ProjetosConsultoria",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Proposito",
                table: "ProjetosConsultoria",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "EstimativasCustos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjetoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Item = table.Column<string>(type: "text", nullable: false),
                    EstimativaHoras = table.Column<int>(type: "integer", nullable: false),
                    ValorHoras = table.Column<double>(type: "double precision", nullable: false),
                    CustoEstimado = table.Column<double>(type: "double precision", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    ProjetoConsultoriaId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstimativasCustos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstimativasCustos_ProjetosConsultoria_ProjetoConsultoriaId",
                        column: x => x.ProjetoConsultoriaId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EstimativasCustos_ProjetosConsultoria_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FaseProjetos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjetoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Fase = table.Column<string>(type: "text", nullable: false),
                    Objetivo = table.Column<string>(type: "text", nullable: false),
                    DuracaoEstimada = table.Column<int>(type: "integer", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    ProjetoConsultoriaId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaseProjetos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaseProjetos_ProjetosConsultoria_ProjetoConsultoriaId",
                        column: x => x.ProjetoConsultoriaId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FaseProjetos_ProjetosConsultoria_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonasChaves",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjetoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Persona = table.Column<string>(type: "text", nullable: false),
                    Perfil = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    Necessidade = table.Column<string>(type: "text", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    ProjetoConsultoriaId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonasChaves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonasChaves_ProjetosConsultoria_ProjetoConsultoriaId",
                        column: x => x.ProjetoConsultoriaId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonasChaves_ProjetosConsultoria_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "ProjetosConsultoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstimativasCustos_ProjetoConsultoriaId",
                table: "EstimativasCustos",
                column: "ProjetoConsultoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_EstimativasCustos_ProjetoId",
                table: "EstimativasCustos",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_FaseProjetos_ProjetoConsultoriaId",
                table: "FaseProjetos",
                column: "ProjetoConsultoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_FaseProjetos_ProjetoId",
                table: "FaseProjetos",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonasChaves_ProjetoConsultoriaId",
                table: "PersonasChaves",
                column: "ProjetoConsultoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonasChaves_ProjetoId",
                table: "PersonasChaves",
                column: "ProjetoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstimativasCustos");

            migrationBuilder.DropTable(
                name: "FaseProjetos");

            migrationBuilder.DropTable(
                name: "PersonasChaves");

            migrationBuilder.DropColumn(
                name: "Observacoes",
                table: "TarefasProjeto");

            migrationBuilder.AlterColumn<string>(
                name: "SituacaoAtual",
                table: "ProjetosConsultoria",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "Proposito",
                table: "ProjetosConsultoria",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevInsight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NovoCampoSituacaoAtual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SituacaoAtual",
                table: "ProjetosConsultoria",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SituacaoAtual",
                table: "ProjetosConsultoria");
        }
    }
}

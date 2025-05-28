using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevInsight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewEntregavelGeradoEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Arquivo",
                table: "EntregaveisGerados",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Conteudo",
                table: "EntregaveisGerados",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomeArquivo",
                table: "EntregaveisGerados",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UrlDownload",
                table: "EntregaveisGerados",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Arquivo",
                table: "EntregaveisGerados");

            migrationBuilder.DropColumn(
                name: "Conteudo",
                table: "EntregaveisGerados");

            migrationBuilder.DropColumn(
                name: "NomeArquivo",
                table: "EntregaveisGerados");

            migrationBuilder.DropColumn(
                name: "UrlDownload",
                table: "EntregaveisGerados");
        }
    }
}

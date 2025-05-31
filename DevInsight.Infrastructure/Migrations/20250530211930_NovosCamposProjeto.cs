using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevInsight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NovosCamposProjeto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataEntrega",
                table: "ProjetosConsultoria",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Proposito",
                table: "ProjetosConsultoria",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataEntrega",
                table: "ProjetosConsultoria");

            migrationBuilder.DropColumn(
                name: "Proposito",
                table: "ProjetosConsultoria");
        }
    }
}

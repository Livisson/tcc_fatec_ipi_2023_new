using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestaoComercio.Infra.Data.Migrations
{
    public partial class alteracaoDespesa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataVencimento",
                table: "Despesa");

            migrationBuilder.AddColumn<int>(
                name: "DiaVencimento",
                table: "Despesa",
                type: "int",
                nullable: false,
                defaultValue: 5);

            migrationBuilder.CreateTable(
                name: "DespesaHistorico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Valor = table.Column<double>(type: "float", nullable: false),
                    Funcao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaVencimento = table.Column<int>(type: "int", nullable: false),
                    DataHistorico = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DespesaHistorico", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DespesaHistorico");

            migrationBuilder.DropColumn(
                name: "DiaVencimento",
                table: "Despesa");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataVencimento",
                table: "Despesa",
                type: "datetime2",
                nullable: true);
        }
    }
}

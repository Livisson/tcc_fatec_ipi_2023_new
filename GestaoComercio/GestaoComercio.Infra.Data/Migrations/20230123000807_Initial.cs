using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestaoComercio.Infra.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Caixa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValorVenda = table.Column<double>(type: "float", nullable: false),
                    DataVenda = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caixa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Despesa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Valor = table.Column<double>(type: "float", nullable: false),
                    Funcao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataVencimento = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Despesa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedor",
                columns: table => new
                {
                    Cnpj = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedor", x => x.Cnpj);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    CodigoBarras = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FornecedorCpnj = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QtdEstoqueTotal = table.Column<int>(type: "int", nullable: false),
                    PerDesconto = table.Column<double>(type: "float", nullable: false),
                    PerMargem = table.Column<double>(type: "float", nullable: false),
                    ValorSugerido = table.Column<double>(type: "float", nullable: false),
                    ValorVenda = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => new { x.CodigoBarras, x.FornecedorCpnj });
                    table.ForeignKey(
                        name: "FK_Produto_Fornecedor_FornecedorCpnj",
                        column: x => x.FornecedorCpnj,
                        principalTable: "Fornecedor",
                        principalColumn: "Cnpj",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EspecificacaoProduto",
                columns: table => new
                {
                    CodigoFornecedorProduto = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodigoBarrasProduto = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ValorCompraProduto = table.Column<double>(type: "float", nullable: false),
                    QtdEstoque = table.Column<int>(type: "int", nullable: false),
                    EmEstoque = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EspecificacaoProduto", x => new { x.CodigoBarrasProduto, x.CodigoFornecedorProduto, x.ValorCompraProduto });
                    table.ForeignKey(
                        name: "FK_EspecificacaoProduto_Produto_CodigoBarrasProduto_CodigoFornecedorProduto",
                        columns: x => new { x.CodigoBarrasProduto, x.CodigoFornecedorProduto },
                        principalTable: "Produto",
                        principalColumns: new[] { "CodigoBarras", "FornecedorCpnj" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoFornecedorProduto = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodigoBarrasProduto = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ValorCompra = table.Column<double>(type: "float", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataCompra = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido", x => new { x.Id, x.CodigoBarrasProduto, x.CodigoFornecedorProduto });
                    table.ForeignKey(
                        name: "FK_Pedido_Produto_CodigoBarrasProduto_CodigoFornecedorProduto",
                        columns: x => new { x.CodigoBarrasProduto, x.CodigoFornecedorProduto },
                        principalTable: "Produto",
                        principalColumns: new[] { "CodigoBarras", "FornecedorCpnj" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdutosVenda",
                columns: table => new
                {
                    CodigoFornecedorProduto = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodigoBarrasProduto = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ValorVendaProduto = table.Column<double>(type: "float", nullable: false),
                    CaixaId = table.Column<int>(type: "int", nullable: false),
                    DataVenda = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Lucro = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutosVenda", x => new { x.CodigoBarrasProduto, x.CodigoFornecedorProduto, x.ValorVendaProduto, x.CaixaId });
                    table.ForeignKey(
                        name: "FK_ProdutosVenda_Caixa_CaixaId",
                        column: x => x.CaixaId,
                        principalTable: "Caixa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutosVenda_Produto_CodigoBarrasProduto_CodigoFornecedorProduto",
                        columns: x => new { x.CodigoBarrasProduto, x.CodigoFornecedorProduto },
                        principalTable: "Produto",
                        principalColumns: new[] { "CodigoBarras", "FornecedorCpnj" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_CodigoBarrasProduto_CodigoFornecedorProduto",
                table: "Pedido",
                columns: new[] { "CodigoBarrasProduto", "CodigoFornecedorProduto" });

            migrationBuilder.CreateIndex(
                name: "IX_Produto_FornecedorCpnj",
                table: "Produto",
                column: "FornecedorCpnj");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosVenda_CaixaId",
                table: "ProdutosVenda",
                column: "CaixaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Despesa");

            migrationBuilder.DropTable(
                name: "EspecificacaoProduto");

            migrationBuilder.DropTable(
                name: "Pedido");

            migrationBuilder.DropTable(
                name: "ProdutosVenda");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Caixa");

            migrationBuilder.DropTable(
                name: "Produto");

            migrationBuilder.DropTable(
                name: "Fornecedor");
        }
    }
}

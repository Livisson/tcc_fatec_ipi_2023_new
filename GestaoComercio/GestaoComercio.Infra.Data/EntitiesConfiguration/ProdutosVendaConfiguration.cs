using GestaoComercio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Infra.Data.EntitiesConfiguration
{
    public class ProdutosVendaConfiguration : IEntityTypeConfiguration<ProdutosVenda>
    {
        public void Configure(EntityTypeBuilder<ProdutosVenda> builder)
        {
            builder.HasKey(t => new { t.CodigoBarrasProduto, t.CodigoFornecedorProduto, t.ValorVendaProduto, t.CaixaId });
            builder.Property(p => p.DataVenda);
            builder.Property(p => p.Quantidade);
            builder.Property(p => p.Lucro);

            builder.HasOne(e => e.Produto).WithMany(e => e.ProdutosVenda)
                .HasForeignKey(e => new { e.CodigoBarrasProduto, e.CodigoFornecedorProduto });

            builder.HasOne(e => e.Caixa).WithMany(e => e.ProdutosVenda)
                .HasForeignKey(e => new { e.CaixaId });

            builder.Navigation(x => x.Produto).AutoInclude();
            builder.Navigation(x => x.Caixa).AutoInclude();

            //builder.HasData(
            //  new Caixa(1, "Material Escolar"),
            //  new Caixa(2, "Eletrônicos"),
            //  new Caixa(3, "Acessórios")
            //);
        }
    }


}

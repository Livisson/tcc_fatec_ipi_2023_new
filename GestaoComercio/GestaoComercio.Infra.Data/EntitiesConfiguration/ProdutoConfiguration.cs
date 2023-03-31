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
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(t => new { t.CodigoBarras, t.FornecedorCpnj });
            builder.Property(p => p.Nome);
            builder.Property(p => p.QtdEstoqueTotal);
            builder.Property(p => p.PerDesconto);
            builder.Property(p => p.PerMargem);
            builder.Property(p => p.ValorSugerido);
            builder.Property(p => p.ValorVenda);

            builder.HasOne(x => x.Fornecedor).WithMany(y => y.ProdutosDoFornecedor)
                .HasForeignKey(z => z.FornecedorCpnj);

            builder.Navigation(x => x.Fornecedor).AutoInclude();
            builder.Navigation(x => x.EspecificacoesDeProduto).AutoInclude();

            //builder.HasData(
            //  new Caixa(1, "Material Escolar"),
            //  new Caixa(2, "Eletrônicos"),
            //  new Caixa(3, "Acessórios")
            //);
        }
    }


}

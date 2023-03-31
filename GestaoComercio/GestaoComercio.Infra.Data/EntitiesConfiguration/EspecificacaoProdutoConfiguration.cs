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
    public class EspecificacaoProdutoConfiguration : IEntityTypeConfiguration<EspecificacaoProduto>
    {
        public void Configure(EntityTypeBuilder<EspecificacaoProduto> builder)
        {
            builder.HasKey(t => new { t.CodigoBarrasProduto, t.CodigoFornecedorProduto, t.ValorCompraProduto });
            builder.Property(p => p.QtdEstoque);
            builder.Property(p => p.EmEstoque);

            builder.HasOne(e => e.Produto).WithMany(e => e.EspecificacoesDeProduto)
                .HasForeignKey(e => new { e.CodigoBarrasProduto, e.CodigoFornecedorProduto });

            //builder.Navigation(x => x.Produto).AutoInclude();

            //builder.HasData(
            //  new Caixa(1, "Material Escolar"),
            //  new Caixa(2, "Eletrônicos"),
            //  new Caixa(3, "Acessórios")
            //);
        }
    }


}

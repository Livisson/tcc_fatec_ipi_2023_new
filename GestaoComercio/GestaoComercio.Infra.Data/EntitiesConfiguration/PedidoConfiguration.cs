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
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(t => new { t.Id, t.CodigoBarrasProduto, t.CodigoFornecedorProduto });
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Quantidade);
            builder.Property(p => p.DataVencimento);
            builder.Property(p => p.DataCompra);

            builder.HasOne(e => e.Produto).WithMany(e => e.PedidosDeProduto)
                .HasForeignKey(e => new { e.CodigoBarrasProduto, e.CodigoFornecedorProduto });

            builder.Navigation(x => x.Produto).AutoInclude();

            //builder.HasData(
            //  new Caixa(1, "Material Escolar"),
            //  new Caixa(2, "Eletrônicos"),
            //  new Caixa(3, "Acessórios")
            //);
        }
    }


}

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
    public class DespesaHistoricoConfiguration : IEntityTypeConfiguration<DespesaHistorico>
    {
        public void Configure(EntityTypeBuilder<DespesaHistorico> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(p => p.Tipo);
            builder.Property(p => p.Descricao);
            builder.Property(p => p.Valor);
            builder.Property(p => p.Funcao);
            builder.Property(p => p.DiaVencimento);
            builder.Property(p => p.DataHistorico);

            //builder.Navigation(x => x.ProdutosDoFornecedor).AutoInclude();
            //builder.HasData(
            //  new Caixa(1, "Material Escolar"),
            //  new Caixa(2, "Eletrônicos"),
            //  new Caixa(3, "Acessórios")
            //);
        }
    }


}

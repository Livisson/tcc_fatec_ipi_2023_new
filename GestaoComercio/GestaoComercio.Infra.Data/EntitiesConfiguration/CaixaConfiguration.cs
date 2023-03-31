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
    public class CaixaConfiguration : IEntityTypeConfiguration<Caixa>
    {
        public void Configure(EntityTypeBuilder<Caixa> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(p => p.ValorVenda);
            builder.Property(p => p.DataVenda);

            //builder.HasData(
            //  new Caixa(1, "Material Escolar"),
            //  new Caixa(2, "Eletrônicos"),
            //  new Caixa(3, "Acessórios")
            //);
        }
    }


}

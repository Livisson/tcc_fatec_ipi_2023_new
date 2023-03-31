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
    public class NomeProdutosConfiguration : IEntityTypeConfiguration<NomeProdutos>
    {
        public void Configure(EntityTypeBuilder<NomeProdutos> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(p => p.NomeProduto);

        }
    }


}

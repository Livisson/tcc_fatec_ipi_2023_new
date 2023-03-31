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
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(p => p.Nome);
            builder.Property(p => p.Senha);
            builder.Property(p => p.Email);

            //builder.Navigation(x => x.ProdutosDoFornecedor).AutoInclude();
            //builder.HasData(
            //  new Caixa(1, "Material Escolar"),
            //  new Caixa(2, "Eletrônicos"),
            //  new Caixa(3, "Acessórios")
            //);
        }
    }


}

using GestaoComercio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Infra.Data.Context
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {

        }

        public DbSet<Caixa> Caixa { get; set; }
        public DbSet<Despesa> Despesa { get; set; }
        public DbSet<DespesaHistorico> DespesaHistorico { get; set; }
        public DbSet<EspecificacaoProduto> EspecificacaoProduto { get; set; }
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<ProdutosVenda> ProdutosVenda { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<NomeProdutos> NomeProdutos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}

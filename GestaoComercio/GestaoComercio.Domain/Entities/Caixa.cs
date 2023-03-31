using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public sealed class Caixa
    {
        public int Id { get; private set; }
        public double ValorVenda { get; private set; }
        public DateTime DataVenda { get; private set; }
        public ICollection<ProdutosVenda> ProdutosVenda { get; set; }

        public Caixa(int id, double valorVenda, DateTime dataVenda)
        {
            DomainExceptionValidation.When(id < 0, "Invalid Id value");
            Id = id;
            ValidateDomain(valorVenda, dataVenda);
        }
        public void Update(double valorVenda, DateTime dataVenda)
        { 
            ValidateDomain(valorVenda, dataVenda);
        }
        private void ValidateDomain(double valorVenda, DateTime dataVenda)
        {

            DomainExceptionValidation.When(double.IsNaN(valorVenda),
                "Valor Unitario inválido. O Valor Unitario é obrigatorio");

            DomainExceptionValidation.When(valorVenda < 0,
                "Valor Unitario inválido. O Valor Unitario não pode ser negativo");

            ValorVenda = valorVenda;
            DataVenda = dataVenda;
            
        }
    }
}

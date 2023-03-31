using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public sealed class Pedido
    {
        public int Id { get; private set; }
        public string CodigoFornecedorProduto { get; set; }
        public string CodigoBarrasProduto { get; set; }
        public Produto Produto { get; set; }
        public double ValorCompra { get; private set; }
        public int Quantidade { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public DateTime DataCompra { get; private set; }

        public Pedido(double valorCompra, int quantidade, DateTime dataVencimento, DateTime dataCompra)
        {
            ValidateDomain(valorCompra, quantidade, dataVencimento, dataCompra);
        }
        public void Update(double valorCompra, int quantidade, DateTime dataVencimento, DateTime dataCompra)
        {
            ValidateDomain(valorCompra, quantidade, dataVencimento, dataCompra);
        }
        private void ValidateDomain(double valorCompra, int quantidade, DateTime dataVencimento, DateTime dataCompra)
        {

            DomainExceptionValidation.When(double.IsNaN(valorCompra),
                "Valor Unitario inválido. O Valor Unitario é obrigatorio");

            DomainExceptionValidation.When(valorCompra < 0,
                "Valor Unitario inválido. O Valor Unitario não pode ser negativo");

            DomainExceptionValidation.When(double.IsNaN(quantidade),
                "Quantidade inválida. A Quantidade é obrigatorio");

            DomainExceptionValidation.When(quantidade < 0,
                "Quantidade inválida. A Quantidade não pode ser negativo");

            ValorCompra = valorCompra;
            Quantidade = quantidade;
            DataVencimento = dataVencimento;
            DataCompra = dataCompra;
            
        }
    }
}

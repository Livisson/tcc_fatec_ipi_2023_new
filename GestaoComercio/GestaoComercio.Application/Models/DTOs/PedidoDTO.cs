using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        public string CodigoFornecedorProduto { get; set; }
        public string CodigoBarrasProduto { get; set; }
        //public ProdutoDTO Produto { get; set; }
        public double ValorCompra { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataCompra { get; set; }

        public void ValidateDomain()
        {

            DomainExceptionValidation.When(double.IsNaN(ValorCompra),
                "Valor Unitario inválido. O Valor Unitario é obrigatorio");

            DomainExceptionValidation.When(ValorCompra < 0,
                "Valor Unitario inválido. O Valor Unitario não pode ser negativo");

            DomainExceptionValidation.When(double.IsNaN(Quantidade),
                "Quantidade inválida. A Quantidade é obrigatorio");

            DomainExceptionValidation.When(Quantidade < 0,
                "Quantidade inválida. A Quantidade não pode ser negativo");
            
        }
    }
}

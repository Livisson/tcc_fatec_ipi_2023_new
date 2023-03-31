using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public class ProdutosVendaDTO
    {
        public string CodigoFornecedorProduto { get; set; }
        public string CodigoBarrasProduto { get; set; }
        public ProdutoDTO Produto { get; set; }
        public double ValorVendaProduto { get; set; }
        public int CaixaId { get; set; }
        public CaixaDTO Caixa { get; set; }
        public DateTime DataVenda { get; set; }
        public int Quantidade { get; set; }
        public double Lucro { get; set; }

        public void ValidateDomain()
        {

            DomainExceptionValidation.When(Quantidade < 0,
                "Quantidade inválido. Quantidade não pode ser negativo");

            DomainExceptionValidation.When(double.IsNaN(Quantidade),
                "Quantidade inválido. Quantidade é obrigatorio");

            DomainExceptionValidation.When(double.IsNaN(Lucro),
                "Lucro inválido. Lucro é obrigatorio");

        }
    }
}

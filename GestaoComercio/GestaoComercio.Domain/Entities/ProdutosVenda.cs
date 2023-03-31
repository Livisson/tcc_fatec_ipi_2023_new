using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public sealed class ProdutosVenda
    {
        public string CodigoFornecedorProduto { get; set; }
        public string CodigoBarrasProduto { get; set; }
        public Produto Produto { get; set; }
        public double ValorVendaProduto { get; set; }
        public int CaixaId { get; set; }
        public Caixa Caixa { get; set; }
        public DateTime DataVenda { get; private set; }
        public int Quantidade { get; private set; }
        public double Lucro { get; private set; }

        public ProdutosVenda(DateTime dataVenda, int quantidade, double lucro)
        {
            ValidateDomain(dataVenda, quantidade, lucro);
        }
        public void Update(DateTime dataVenda, int quantidade, double lucro)
        {
            ValidateDomain(dataVenda, quantidade, lucro);
        }
        private void ValidateDomain(DateTime dataVenda, int quantidade, double lucro)
        {

            DomainExceptionValidation.When(quantidade < 0,
                "Quantidade inválido. Quantidade não pode ser negativo");

            DomainExceptionValidation.When(double.IsNaN(quantidade),
                "Quantidade inválido. Quantidade é obrigatorio");

            DomainExceptionValidation.When(double.IsNaN(lucro),
                "Lucro inválido. Lucro é obrigatorio");

            DataVenda = dataVenda;
            Quantidade = quantidade;
            Lucro = lucro;
        }
    }
}

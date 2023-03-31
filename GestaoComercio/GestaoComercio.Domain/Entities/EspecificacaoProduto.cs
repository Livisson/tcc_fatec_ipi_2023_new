using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public class EspecificacaoProduto
    {
        public string CodigoFornecedorProduto { get; set; }
        public string CodigoBarrasProduto { get; set; }
        public double ValorCompraProduto { get; set; }
        public virtual Produto Produto { get; set; }
        public int QtdEstoque { get; private set; }
        public bool EmEstoque { get; private set; }

        public EspecificacaoProduto(int qtdEstoque, bool emEstoque)
        {
            ValidateDomain(qtdEstoque, emEstoque);
        }
        public void Update(int qtdEstoque, bool emEstoque)
        {
            ValidateDomain(qtdEstoque, emEstoque);
        }
        private void ValidateDomain(int qtdEstoque, bool emEstoque)
        {

            DomainExceptionValidation.When(qtdEstoque < 0,
                "Quantidade inválido. Quantidade não pode ser negativo");

            DomainExceptionValidation.When(double.IsNaN(qtdEstoque),
                "Quantidade inválido. Quantidade é obrigatorio");


            QtdEstoque = qtdEstoque;
            EmEstoque = emEstoque;
        }
    }
}

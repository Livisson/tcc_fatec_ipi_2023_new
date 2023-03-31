using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public class EspecificacaoProdutoDTO
    {
        public string CodigoFornecedorProduto { get; set; }
        public string CodigoBarrasProduto { get; set; }
        public double ValorCompraProduto { get; set; }
        //public ProdutoDTO Produto { get; set; }
        public int QtdEstoque { get; set; }
        public bool EmEstoque { get; set; }

        public void ValidateDomain()
        {

            DomainExceptionValidation.When(QtdEstoque < 0,
                "Quantidade inválido. Quantidade não pode ser negativo");

            DomainExceptionValidation.When(double.IsNaN(QtdEstoque),
                "Quantidade inválido. Quantidade é obrigatorio");

        }
    }
}

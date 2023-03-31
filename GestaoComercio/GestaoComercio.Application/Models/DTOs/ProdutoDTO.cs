using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public class ProdutoDTO
    {
        public string Nome { get; set; }
        public string CodigoBarras { get; set; }
        public string FornecedorCpnj { get; set; }
        public FornecedorDTO Fornecedor { get; set; }
        public int QtdEstoqueTotal { get; set; }
        public double PerDesconto { get; set; }
        public double PerMargem { get; set; }
        public double ValorSugerido { get; set; }
        public double ValorVenda { get; set; }
        public ICollection<PedidoDTO> PedidosDeProduto { get; set; }
        public ICollection<EspecificacaoProdutoDTO> EspecificacoesDeProduto { get; set; }
        public ICollection<ProdutosVendaDTO> ProdutosVenda { get; set; }

        private void ValidateDomain()
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(Nome),
                "Nome inválido. O nome é obrigatorio");

            DomainExceptionValidation.When(Nome.Length < 3,
                "Nome invalido. Muito pequeno, minimo 3 caracteres");

            DomainExceptionValidation.When(string.IsNullOrEmpty(CodigoBarras),
                "Codigo Barras inválido. O codigo de barras é obrigatorio");

            DomainExceptionValidation.When(CodigoBarras.Length < 5,
                "Codigo Barras invalido. Muito pequeno, minimo 5 caracteres");

            DomainExceptionValidation.When(double.IsNaN(QtdEstoqueTotal),
                "Quantidade inválida. A Quantidade é obrigatorio");

            DomainExceptionValidation.When(QtdEstoqueTotal < 0,
                "Quantidade inválida. A Quantidade não pode ser negativo");

            DomainExceptionValidation.When(PerDesconto < 0,
                "Porcentagem Desconto. Porcentagem Desconto não pode ser negativo");

            DomainExceptionValidation.When(double.IsNaN(PerMargem),
                "Porcentagem Margem inválida. Porcentagem Margem é obrigatorio");

            DomainExceptionValidation.When(PerMargem < 0,
                "Porcentagem Margem inválida. Porcentagem Margem não pode ser negativo");

            DomainExceptionValidation.When(double.IsNaN(ValorSugerido),
                "Valor Sugerido inválido. O Valor Sugerido é obrigatorio");

            DomainExceptionValidation.When(ValorSugerido < 0,
                "Valor Sugerido inválido. O Valor Sugerido não pode ser negativo");

            DomainExceptionValidation.When(double.IsNaN(ValorVenda),
                "Valor Venda inválido. O Valor Venda é obrigatorio");

            DomainExceptionValidation.When(ValorVenda < 0,
                "Valor Venda inválido. O Valor Venda não pode ser negativo");

        }

        public void RegraNegocio()
        {
            QtdEstoqueTotal = EspecificacoesDeProduto.Where(x => x.EmEstoque).Sum(x => x.QtdEstoque);
            var valorComMargem = EspecificacoesDeProduto.Where(x => x.EmEstoque).Max(x => x.ValorCompraProduto) * (1 + (PerMargem / 100));
            ValorSugerido = valorComMargem - valorComMargem * (PerDesconto / 100);
        }
    }
}

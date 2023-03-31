using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public class Produto
    {
        public string Nome { get; set; }
        public string CodigoBarras { get; set; }
        public string FornecedorCpnj { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public int QtdEstoqueTotal { get; set; }
        public double PerDesconto { get; set; }
        public double PerMargem { get; set; }
        public double ValorSugerido { get; set; }
        public double ValorVenda { get; set; }
        public virtual ICollection<Pedido> PedidosDeProduto { get; set; }
        public virtual ICollection<EspecificacaoProduto> EspecificacoesDeProduto { get; set; }
        //public virtual ICollection<EspecificacaoProduto> EspecificacoesDeProduto { get => _especificacoesDeProduto; set => _especificacoesDeProduto = value; }
        //private ICollection<EspecificacaoProduto> _especificacoesDeProduto { get; set; }
        public virtual ICollection<ProdutosVenda> ProdutosVenda { get; set; }
        public Produto(string nome, string codigoBarras, int qtdEstoqueTotal, double perDesconto, double perMargem, double valorSugerido, double valorVenda)
        {
            ValidateDomain(nome, codigoBarras, qtdEstoqueTotal, perDesconto, perMargem, valorSugerido, valorVenda);
        }
        public void Update(string nome, string codigoBarras, int qtdEstoqueTotal, double perDesconto, double perMargem, double valorSugerido, double valorVenda)
        {
            ValidateDomain(nome, codigoBarras, qtdEstoqueTotal, perDesconto, perMargem, valorSugerido, valorVenda);
        }
        private void ValidateDomain(string nome, string codigoBarras, int qtdEstoqueTotal, double perDesconto, double perMargem, double valorSugerido, double valorVenda)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(nome),
                "Nome inválido. O nome é obrigatorio");

            DomainExceptionValidation.When(nome.Length < 3,
                "Nome invalido. Muito pequeno, minimo 3 caracteres");

            DomainExceptionValidation.When(string.IsNullOrEmpty(codigoBarras),
                "Codigo Barras inválido. O codigo de barras é obrigatorio");

            DomainExceptionValidation.When(codigoBarras.Length < 5,
                "Codigo Barras invalido. Muito pequeno, minimo 5 caracteres");

            DomainExceptionValidation.When(double.IsNaN(qtdEstoqueTotal),
                "Quantidade inválida. A Quantidade é obrigatorio");

            DomainExceptionValidation.When(qtdEstoqueTotal < 0,
                "Quantidade inválida. A Quantidade não pode ser negativo");

            DomainExceptionValidation.When(perDesconto < 0,
                "Porcentagem Desconto. Porcentagem Desconto não pode ser negativo");

            DomainExceptionValidation.When(double.IsNaN(perMargem),
                "Porcentagem Margem inválida. Porcentagem Margem é obrigatorio");

            DomainExceptionValidation.When(perMargem < 0,
                "Porcentagem Margem inválida. Porcentagem Margem não pode ser negativo");

            DomainExceptionValidation.When(double.IsNaN(valorSugerido),
                "Valor Sugerido inválido. O Valor Sugerido é obrigatorio");

            DomainExceptionValidation.When(valorSugerido < 0,
                "Valor Sugerido inválido. O Valor Sugerido não pode ser negativo");

            DomainExceptionValidation.When(double.IsNaN(valorVenda),
                "Valor Venda inválido. O Valor Venda é obrigatorio");

            DomainExceptionValidation.When(valorVenda < 0,
                "Valor Venda inválido. O Valor Venda não pode ser negativo");

            Nome = nome;
            CodigoBarras = codigoBarras;
            QtdEstoqueTotal = qtdEstoqueTotal;
            PerDesconto = perDesconto;
            PerMargem = perMargem;
            ValorSugerido = valorSugerido;
            ValorVenda = valorVenda;
        }
    }
}

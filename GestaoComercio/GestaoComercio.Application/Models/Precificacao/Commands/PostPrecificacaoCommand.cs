using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoComercio.Application.Models.Precificacao.Commands
{
    public class PostPrecificacaoCommand
    {
        public string NomeProduto { get; set; }
        public string CodigoBarras { get; set; }
        public string CodigoFornecedor { get; set; }
        public double ValorCompra { get; set; }
        public int Estoque { get; set; }
        public double PerDesconto { get; set; }
        public double PerMargem { get; set; }
        public double ValorSugerido { get; set; }
        public double ValorVenda { get; set; }
    }
}

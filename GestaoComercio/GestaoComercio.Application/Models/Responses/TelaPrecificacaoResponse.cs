using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Application.Responses
{
    public class TelaPrecificacaoResponse
    {
        public string NomeProduto { get; set; }
        public string CodigoBarras { get; set; }
        public string CodigoFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public double ValorCompra { get; set; }
        public int Estoque { get; set; }
        public double PerDesconto { get; set; }
        public double PerMargem { get; set; }
        public double ValorSugerido { get; set; }
        public double ValorVenda { get; set; }
    }
}

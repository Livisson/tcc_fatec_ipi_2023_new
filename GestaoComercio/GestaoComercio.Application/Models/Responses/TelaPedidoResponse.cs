using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Application.Responses
{
    public class TelaPedidoResponse
    {
        public string NomeProduto { get; set; }
        public string CodigoBarras { get; set; }
        public string NomeFornecedor { get; set; }
        public double ValorCompra { get; set; }
        public int Quantidade { get; set; }
        public double ValorTotal { get; set; }
        public DateTime DataPagamento { get; set; }
    }
}

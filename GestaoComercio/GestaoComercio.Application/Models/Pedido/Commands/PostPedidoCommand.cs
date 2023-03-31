using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoComercio.Application.Models.Pedido.Commands
{
    public class PostPedidoCommand
    {
        public string CodigoFornecedor { get; set; }
        public string NomeProduto { get; set; }
        public string CodigoBarras { get; set; }
        public double ValorCompra { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataPagamento { get; set; }
    }
}

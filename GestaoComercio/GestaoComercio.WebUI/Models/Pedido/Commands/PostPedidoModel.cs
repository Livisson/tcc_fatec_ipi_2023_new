using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GestaoComercio.WebUI.Models.Pedido.Commands
{
    public class PostPedidoModel : FromBodyAttribute
    {
        //[JsonPropertyName("codigo_fornecedor")]
        public string CodigoFornecedor { get; set; }
        public string NomeProduto { get; set; }
        public string CodigoBarras { get; set; }
        public double ValorCompra { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataPagamento { get; set; }
    }
}

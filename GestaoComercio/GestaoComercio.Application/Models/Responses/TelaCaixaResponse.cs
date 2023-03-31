using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Application.Responses
{
    public class TelaCaixaResponse
    {
        public string NomeProduto { get; set; }
        public string CodigoBarras { get; set; }
        public int Quantidade { get; set; }
        public double ValorVenda { get; set; }
        public double PerDesconto { get; set; }
        public double ValorTotalVenda { get; set; }
    }
}

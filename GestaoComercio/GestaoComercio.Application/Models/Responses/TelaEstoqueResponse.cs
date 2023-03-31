using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Application.Models.Responses
{
    public class TelaEstoqueResponse
    {
        public string NomeProduto { get; set; }
        public string CodigoBarras { get; set; }
        public string NomeFornecedor { get; set; }
        public string CnpjFornecedor { get; set; }
        public int Quantidade { get; set; }
    }
}

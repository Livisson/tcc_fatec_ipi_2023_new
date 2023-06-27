using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Application.Models.Responses
{
    public class RequisicaoChatGPTConsolidado
    {
        public string Data { get; set; }
        public string Receita { get; set; }
        public string Despesa { get; set; }
        public string Saldo { get; set; }
    }
}

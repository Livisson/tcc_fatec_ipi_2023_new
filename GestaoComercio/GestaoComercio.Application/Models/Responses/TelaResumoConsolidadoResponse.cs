using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Application.Models.Responses
{
    public class TelaResumoConsolidadoResponse
    {
        public string Receita { get; set; }
        public string Despesa { get; set; }
        public string ReceitaPrevista { get; set; }
        public string SaldoGeral { get; set; }
    }
}

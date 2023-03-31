using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Application.Responses
{
    public class DespesaResponse
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public string Funcao { get; set; }
        public DateTime? DataVencimento { get; set; }
    }
}

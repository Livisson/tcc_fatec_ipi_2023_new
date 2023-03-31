using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Application.Models.Despesa.Commands
{
    public class PostDespesaCommand
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public string Funcao { get; set; }
        public int DiaVencimento { get; set; }
    }
}

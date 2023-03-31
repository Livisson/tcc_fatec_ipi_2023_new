using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoComercio.WebUI.Models.Despesa.Command
{
    public class PostDespesaModel : FromBodyAttribute
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public string Funcao { get; set; }
        public int DiaVencimento { get; set; }
    }
}

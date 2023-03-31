using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoComercio.WebUI.Models.Caixa.Command
{
    public class PostCaixaModel : FromBodyAttribute
    {
        public string CodigoBarras { get; set; }
        public string NomeProduto { get; set; }
        public int Quantidade { get; set; }

    }
}

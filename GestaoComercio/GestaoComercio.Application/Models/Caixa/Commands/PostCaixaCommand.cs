using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoComercio.Application.Models.Caixa.Commands
{
    public class PostCaixaCommand
    {
        public string CodigoBarras { get; set; }
        public string NomeProduto { get; set; }
        public int Quantidade { get; set; }
    }
}

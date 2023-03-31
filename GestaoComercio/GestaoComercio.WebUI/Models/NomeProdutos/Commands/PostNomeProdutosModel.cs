using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoComercio.WebUI.Models.NomeProdutos.Commands
{
    public class PostNomeProdutosModel : FromBodyAttribute
    {
        public string CnpjFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public string NomeProduto { get; set; }

        public string CodigoBarras { get; set; }
    }
}

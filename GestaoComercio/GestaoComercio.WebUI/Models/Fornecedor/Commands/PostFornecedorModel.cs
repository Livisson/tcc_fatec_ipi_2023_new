using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoComercio.WebUI.Models.Fornecedor.Commands
{
    public class PostFornecedorModel : FromBodyAttribute
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoComercio.WebUI.Models.NomeProdutos.Commands
{
    public class PostNomeProdutosModelOld : FromBodyAttribute
    {
        public int Id { get; set; }
        public string NomeProduto { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoComercio.Application.Models.Usuario.Commands
{
    public class PostUsuarioCommand
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string SenhaAtual { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
    }
}

using AutoMapper;
using GestaoComercio.Application.Models.Usuario.Commands;
using GestaoComercio.Application.Services;
using GestaoComercio.Domain.Entities;
using GestaoComercio.Domain.Interfaces;
using GestaoComercio.WebUI.Models.Usuario.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoComercio.WebUI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Usuario> _usuarioRepository;

        public UsuarioController(IMapper mapper, IGenericRepository<Usuario> usuarioRepository)
        {
            _mapper = mapper;
            _usuarioService = new UsuarioService(usuarioRepository, mapper);
        }


        [HttpGet]
        public async Task<IActionResult> getUsuario()
        {
            return Ok(await _usuarioService.ConsultaUsuarios());
        }

        [HttpPut]
        public async Task<IActionResult> PutUsuario(PostUsuarioModel request) =>
            Ok(await _usuarioService.AtualizarUsuario(_mapper.Map<PostUsuarioCommand>(request)));

        [HttpGet("getRecuperarSenha")]
        public async Task<IActionResult> GetRecuperarSenha() =>
            Ok(await _usuarioService.RecuperarSenha());
    }
}

﻿using AutoMapper;
using GestaoComercio.Application.Services;
using GestaoComercio.Domain.Entities;
using GestaoComercio.Domain.Interfaces;
using GestaoComercio.Domain.Utils;
using GestaoComercio.WebUI.Models.Usuario;
using GestaoComercio.WebUI.TokenManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoComercio.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenManager _tokenManager;
        private readonly UsuarioService _usuarioService;
        private readonly IMapper _mapper;
        public TokenController(IJwtTokenManager jwtTokenManager, IMapper mapper, IGenericRepository<Usuario> usuarioRepository, IConfiguration configuration)
        {
            _tokenManager = jwtTokenManager;
            _usuarioService = new UsuarioService(usuarioRepository, mapper, configuration);
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody]UserCredential credential)
        {
            var dbUser = _usuarioService.GetUsuarioByIndex(credential.UserName, credential.Password.GerarHash());
            if (dbUser == null)
            {
                return Unauthorized();
            }
            var token = _tokenManager.Authenticate(credential.UserName, credential.Password.GerarHash());

            //var dbUser = _usuarioService.ConsultaUsuario();
            //var token = _tokenManager.Authenticate(credential.UserName, credential.Password);
            //if (string.IsNullOrEmpty(token))
            //{
            //    return Unauthorized();
            //}
            return Ok(token);
        }
    }
}

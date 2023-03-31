using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using GestaoComercio.Application.Services;
using AutoMapper;
using GestaoComercio.Domain.Interfaces;
using GestaoComercio.Domain.Entities;

namespace GestaoComercio.WebUI.TokenManager
{
    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly IConfiguration _configuration;
        //private readonly UsuarioService _usuarioService;
        //private readonly IMapper _mapper;
        public JwtTokenManager(IConfiguration configuration)
        {
            _configuration = configuration;
            //_usuarioService = new UsuarioService(usuarioRepository, mapper);
        }
        public string Authenticate(string userName, string password)
        {
            //var dbUser = _usuarioService.ConsultaUsuario();
            //Data.Users.Add(dbUser.Result.FirstOrDefault().Nome, dbUser.Result.FirstOrDefault().Senha);
            //if (!Data.Users.Any(x => x.Key.Equals(userName) && x.Value.Equals(password)))
            //{
            //    return null;
            //}

            var key = _configuration.GetValue<string>("JwtConfig:Key");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userName)
                }),
                Expires = DateTime.UtcNow.AddDays(90),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

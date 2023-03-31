using AutoMapper;
using GestaoComercio.Application.Models.Fornecedor.Commands;
using GestaoComercio.Application.Models.Pedido.Commands;
using GestaoComercio.Application.Models.Usuario.Commands;
using GestaoComercio.Application.Responses;
using GestaoComercio.Domain.Entities;
using GestaoComercio.Domain.Interfaces;
using GestaoComercio.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Application.Services
{
    public class UsuarioService
    {

        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IGenericRepository<Usuario> usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public UsuarioDTO GetUsuarioByIndex(string nome, string senha)
        {
            return _mapper.Map<UsuarioDTO>(_usuarioRepository.Get(x => x.Nome == nome && x.Senha == senha));
        }
        public async Task<UsuarioResponse> ConsultaUsuarios()
        {
            var usuario = await _usuarioRepository.GetAsync();
            UsuarioResponse response = new UsuarioResponse
            {
                Id = usuario.FirstOrDefault().Id,
                Nome = usuario.FirstOrDefault().Nome,
                Senha = "********",
                Email = usuario.FirstOrDefault().Email
            };

            return response;
        }

        public async Task<UsuarioDTO> AtualizarUsuario(PostUsuarioCommand request)
        {
            return _mapper.Map<UsuarioDTO>(await _usuarioRepository.UpdateAsync(_mapper.Map<Usuario>(request)));
        }

        public async Task<string> RecuperarSenha()
        {
            try
            {
                var usuario = await _usuarioRepository.GetAsync();

                // Configura as informações do remetente
                string remetente = "provap2pweb@outlook.com";
                string senha = "Pwebp22022";
                //string destinatario = "gestaocomercio.tccfatec@gmail.com";
                string destinatario = usuario.FirstOrDefault().Email;
                string assunto = "Recuperar Senha - Sistema Gestão de Comércio";
                string mensagem = "O Usuário cadastrado é: Usuário - " + usuario.FirstOrDefault().Nome + " / Senha - " + usuario.FirstOrDefault().Senha;

                // Configura o cliente SMTP
                SmtpClient cliente = new SmtpClient("smtp-mail.outlook.com", 587);
                cliente.EnableSsl = true;
                cliente.UseDefaultCredentials = false;
                cliente.Credentials = new System.Net.NetworkCredential(remetente, senha);

                // Configura o e-mail
                MailMessage email = new MailMessage();
                email.From = new MailAddress(remetente);
                email.To.Add(destinatario);
                email.Subject = assunto;
                email.Body = mensagem;

                // Envia o e-mail
                cliente.Send(email);

                return "E-mail enviado com sucesso!";
            }
            catch (Exception ex)
            {
                throw new MyExceptionApi("Erro ao enviar o e-mail", HttpStatusCode.InternalServerError);
            }

        }
    }
}

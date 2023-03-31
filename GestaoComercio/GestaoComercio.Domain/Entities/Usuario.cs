using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public sealed class Usuario
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Senha { get; private set; }
        public string Email { get; private set; }

        public Usuario(int id, string nome, string senha, string email)
        {
            DomainExceptionValidation.When(id < 0, "Invalid Id value");
            Id = id;
            ValidateDomain(nome, senha, email);
        }
        public void Update(string nome, string senha, string email)
        {
            ValidateDomain(nome, senha, email);
        }
        private void ValidateDomain(string nome, string senha, string email)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(nome),
                "Nome inválido. O nome é obrigatorio");

            DomainExceptionValidation.When(nome.Length < 3,
                "Nome invalido. Muito pequeno, minimo 3 caracteres");

            DomainExceptionValidation.When(string.IsNullOrEmpty(senha),
                "Senha inválida. A senha é obrigatoria");

            DomainExceptionValidation.When(senha.Length < 5,
                "Senha invalida. Muito pequeno, minimo 5 caracteres");

            DomainExceptionValidation.When(string.IsNullOrEmpty(email),
                "Email inválido. O email é obrigatorio");

            DomainExceptionValidation.When(email.Length < 5,
                "Email invalido. Muito pequeno, minimo 5 caracteres");

            Nome = nome;
            Senha = senha;
            Email = email;
        }
    }
}

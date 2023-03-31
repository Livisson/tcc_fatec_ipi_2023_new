using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }

        public void ValidateDomain()
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(Nome),
                "Nome inválido. O nome é obrigatorio");

            DomainExceptionValidation.When(Nome.Length < 3,
                "Nome invalido. Muito pequeno, minimo 3 caracteres");

            DomainExceptionValidation.When(string.IsNullOrEmpty(Senha),
                "Senha inválida. A senha é obrigatoria");

            DomainExceptionValidation.When(Senha.Length < 5,
                "Senha invalida. Muito pequeno, minimo 5 caracteres");

            DomainExceptionValidation.When(string.IsNullOrEmpty(Email),
                "Email inválido. O email é obrigatorio");

            DomainExceptionValidation.When(Email.Length < 5,
                "Email invalido. Muito pequeno, minimo 5 caracteres");
        }
    }
}

using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public class FornecedorDTO
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }

        private void ValidateDomain()
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(Nome),
                "Nome inválido. O nome é obrigatorio");

            DomainExceptionValidation.When(Nome.Length < 3,
                "Nome invalido. Muito pequeno, minimo 3 caracteres");

            DomainExceptionValidation.When(string.IsNullOrEmpty(Cnpj),
                "Cnpj inválido. O cnpj é obrigatoria");

            DomainExceptionValidation.When(Cnpj.Length < 5,
                "Cnpj inválido. Muito pequeno, minimo 5 caracteres");

        }
    }
}

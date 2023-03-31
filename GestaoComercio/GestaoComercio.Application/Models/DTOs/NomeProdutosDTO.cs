using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public class NomeProdutosDTO
    {
        public int Id { get; set; }
        public string NomeProduto { get; set; }

        public void ValidateDomain()
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(NomeProduto),
                "Nome inválido. O nome é obrigatorio");

            DomainExceptionValidation.When(NomeProduto.Length < 3,
                "Nome invalido. Muito pequeno, minimo 3 caracteres");

        }
    }
}

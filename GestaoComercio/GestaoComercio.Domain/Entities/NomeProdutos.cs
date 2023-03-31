using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public sealed class NomeProdutos
    {
        public int Id { get; private set; }
        public string NomeProduto { get; private set; }

        public NomeProdutos(int id, string nomeProduto)
        {
            DomainExceptionValidation.When(id < 0, "Invalid Id value");
            Id = id;
            ValidateDomain(nomeProduto);
        }
        public void Update(string nomeProduto)
        {
            ValidateDomain(nomeProduto);
        }
        private void ValidateDomain(string nomeProduto)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(nomeProduto),
                "Nome inválido. O nome é obrigatorio");

            DomainExceptionValidation.When(nomeProduto.Length < 3,
                "Nome invalido. Muito pequeno, minimo 3 caracteres");

            NomeProduto = nomeProduto;
        }
    }
}

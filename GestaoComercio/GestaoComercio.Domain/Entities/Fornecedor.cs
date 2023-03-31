using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public sealed class Fornecedor
    {
        public string Nome { get; private set; }
        public string Cnpj { get; private set; }
        public ICollection<Produto> ProdutosDoFornecedor { get; set; }

        public Fornecedor(string nome, string cnpj)
        {
            ValidateDomain(nome, cnpj);
        }
        public void Update(string nome, string cnpj)
        {
            ValidateDomain(nome, cnpj);
        }
        private void ValidateDomain(string nome, string cnpj)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(nome),
                "Nome inválido. O nome é obrigatorio");

            DomainExceptionValidation.When(nome.Length < 3,
                "Nome invalido. Muito pequeno, minimo 3 caracteres");

            DomainExceptionValidation.When(string.IsNullOrEmpty(cnpj),
                "Cnpj inválido. O cnpj é obrigatoria");

            DomainExceptionValidation.When(cnpj.Length < 5,
                "Cnpj inválido. Muito pequeno, minimo 5 caracteres");

            Nome = nome;
            Cnpj = cnpj;
        }
    }
}

using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public class DespesaHistoricoDTO
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public string Funcao { get; set; }
        public int DiaVencimento { get; set; }
        public DateTime DataHistorico { get; set; }

        public void ValidateDomain()
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(Tipo),
                "Nome inválido. O nome é obrigatorio");

            DomainExceptionValidation.When(Tipo.Length < 3,
                "Nome invalido. Muito pequeno, minimo 3 caracteres");

            DomainExceptionValidation.When(string.IsNullOrEmpty(Descricao),
                "Descricao inválida. A Descricao é obrigatoria");

            DomainExceptionValidation.When(Descricao.Length < 5,
                "Descricao invalida. Muito pequeno, minimo 5 caracteres");

            DomainExceptionValidation.When(double.IsNaN(Valor),
                "Valor inválida. O Valor é obrigatorio");

            DomainExceptionValidation.When(Valor < 0,
                "Valor inválido. O Valor não pode ser negativo");

            DomainExceptionValidation.When(string.IsNullOrEmpty(Funcao),
                "Função inválida. a Função é obrigatoria");

            DomainExceptionValidation.When(Funcao.Length < 3,
                "Função inválida. Muito pequeno, minimo 3 caracteres");

            DomainExceptionValidation.When(DiaVencimento < 1,
                "Valor inválido. O Valor não pode ser menor que 1");

            DomainExceptionValidation.When(DiaVencimento > 31,
                "Valor inválido. O Valor não pode ser maior que 31");

        }
    }
}

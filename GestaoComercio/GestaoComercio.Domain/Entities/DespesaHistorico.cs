using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public sealed class DespesaHistorico
    {
        public int Id { get; private set; }
        public string Tipo { get; private set; }
        public string Descricao { get; private set; }
        public double Valor { get; private set; }
        public string Funcao { get; private set; }
        public int DiaVencimento { get; private set; }
        public DateTime DataHistorico { get; private set; }

        public DespesaHistorico(int id, string tipo, string descricao, double valor, string funcao, int diaVencimento, DateTime dataHistorico)
        {
            DomainExceptionValidation.When(id < 0, "Invalid Id value");
            Id = id;
            ValidateDomain(tipo, descricao, valor, funcao, diaVencimento, dataHistorico);
        }
        public void Update(string tipo, string descricao, double valor, string funcao, int diaVencimento, DateTime dataHistorico)
        {
            ValidateDomain(tipo, descricao, valor, funcao, diaVencimento, dataHistorico);
        }
        private void ValidateDomain(string tipo, string descricao, double valor, string funcao, int diaVencimento, DateTime dataHistorico)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(tipo),
                "Nome inválido. O nome é obrigatorio");

            DomainExceptionValidation.When(tipo.Length < 3,
                "Nome invalido. Muito pequeno, minimo 3 caracteres");

            DomainExceptionValidation.When(string.IsNullOrEmpty(descricao),
                "Descricao inválida. A Descricao é obrigatoria");

            DomainExceptionValidation.When(descricao.Length < 3,
                "Descricao invalida. Muito pequeno, minimo 3 caracteres");

            DomainExceptionValidation.When(double.IsNaN(valor),
                "Valor inválida. O Valor é obrigatorio");

            DomainExceptionValidation.When(valor < 0,
                "Valor inválido. O Valor não pode ser negativo");

            DomainExceptionValidation.When(diaVencimento < 1,
                "Valor inválido. O Valor não pode ser menor que 1");

            DomainExceptionValidation.When(diaVencimento > 31,
                "Valor inválido. O Valor não pode ser maior que 31");

            Tipo = tipo;
            Descricao = descricao;
            Valor = valor;
            Funcao = funcao;
            DiaVencimento = diaVencimento;
            DataHistorico = dataHistorico;
        }
    }
}

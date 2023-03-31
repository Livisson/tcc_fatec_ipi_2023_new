using GestaoComercio.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Entities
{
    public class CaixaDTO
    {
        public int Id { get; set; }
        public double ValorVenda { get; set; }
        public DateTime DataVenda { get; set; }
        public ICollection<ProdutosVendaDTO> ProdutosVenda { get; set; }

        public void ValidateDomain()
        {

            DomainExceptionValidation.When(double.IsNaN(ValorVenda),
                "Valor Unitario inválido. O Valor Unitario é obrigatorio");

            DomainExceptionValidation.When(ValorVenda < 0,
                "Valor Unitario inválido. O Valor Unitario não pode ser negativo");
            
        }
    }
}

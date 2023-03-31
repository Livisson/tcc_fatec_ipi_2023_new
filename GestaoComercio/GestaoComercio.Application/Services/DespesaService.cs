using AutoMapper;
using GestaoComercio.Application.Models.Despesa.Commands;
using GestaoComercio.Application.Models.Fornecedor.Commands;
using GestaoComercio.Application.Models.Pedido.Commands;
using GestaoComercio.Domain.Entities;
using GestaoComercio.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Application.Services
{
    public class DespesaService
    {

        private readonly IGenericRepository<Despesa> _despesaRepository;
        private readonly IMapper _mapper;

        public DespesaService(IGenericRepository<Despesa> despesaRepository, IMapper mapper)
        {
            _despesaRepository = despesaRepository;
            _mapper = mapper;
        }
        public async Task<DespesaDTO> InserirDespesa(PostDespesaCommand request)
        {
            request.Id = 0;
            var teste = _mapper.Map<Despesa>(request);
            return _mapper.Map<DespesaDTO>(await _despesaRepository.CreateAsync(_mapper.Map<Despesa>(request)));
        }

        public async Task<DespesaDTO> AtualizarDespesa(PostDespesaCommand request)
        {
            return _mapper.Map<DespesaDTO>(await _despesaRepository.UpdateAsync(_mapper.Map<Despesa>(request)));
        }

        public async Task<DespesaDTO> DeletarDespesa(int id)
        {
            var despesa = _despesaRepository.Get(x => x.Id == id);
            return _mapper.Map<DespesaDTO>(await _despesaRepository.RemoveAsync(_mapper.Map<Despesa>(despesa)));
        }

        public async Task<IEnumerable<DespesaDTO>> ConsultaDespesas()
        {
            return _mapper.Map<IEnumerable<DespesaDTO>>(await _despesaRepository.GetAsync());
        }
    }
}

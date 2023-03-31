using AutoMapper;
using GestaoComercio.Application.Models.Fornecedor.Commands;
using GestaoComercio.Application.Models.Pedido.Commands;
using GestaoComercio.Domain.Entities;
using GestaoComercio.Domain.Interfaces;
using GestaoComercio.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Application.Services
{
    public class FornecedorService
    {

        private readonly IGenericRepository<Fornecedor> _fornecedorRepository;
        private readonly IGenericRepository<Pedido> _pedidoRepository;
        private readonly IMapper _mapper;

        public FornecedorService(IGenericRepository<Fornecedor> fornecedorRepository, IMapper mapper, IGenericRepository<Pedido> pedidoRepository)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
            _pedidoRepository = pedidoRepository;
        }
        public async Task<FornecedorDTO> InserirFornecedor(PostFornecedorCommand request)
        {
            var teste = _mapper.Map<Fornecedor>(request);
            return _mapper.Map<FornecedorDTO>(await _fornecedorRepository.CreateAsync(_mapper.Map<Fornecedor>(request)));
        }

        public async Task<FornecedorDTO> AtualizarFornecedor(PostFornecedorCommand request)
        {
            return _mapper.Map<FornecedorDTO>(await _fornecedorRepository.UpdateAsync(_mapper.Map<Fornecedor>(request)));
        }

        public async Task<FornecedorDTO> DeletarFornecedor(string cnpj)
        {
            var pedidos = _pedidoRepository.GetAsync().Result.Where(x => x.CodigoFornecedorProduto == cnpj);
            if (pedidos.Count() > 0)
            {
                throw new MyExceptionApi("Não é possivel deletar o Fornecedor, pois há pedidos atrelados a ele!", HttpStatusCode.BadRequest);
            }

            var fornecedor = _fornecedorRepository.Get(x => x.Cnpj == cnpj.ToString());
            return _mapper.Map<FornecedorDTO>(await _fornecedorRepository.RemoveAsync(fornecedor));
        }

        public async Task<IEnumerable<FornecedorDTO>> ConsultaFornecedores()
        {
            return _mapper.Map<IEnumerable<FornecedorDTO>>(await _fornecedorRepository.GetAsync());
        }

        public FornecedorDTO ConsultaFornecedoresByCnpj(string cnpj)
        {
            return _mapper.Map<FornecedorDTO>(_fornecedorRepository.Get(x => x.Cnpj == cnpj));
        }
    }
}

using AutoMapper;
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
    public class EspecificacoesProdutoService
    {
        private readonly IGenericRepository<EspecificacaoProduto> _especificacaoProdutoRepository;
        private readonly IMapper _mapper;

        public EspecificacoesProdutoService(IGenericRepository<EspecificacaoProduto> especificacaoProdutoRepository, IMapper mapper)
        {
            _especificacaoProdutoRepository = especificacaoProdutoRepository;
            _mapper = mapper;
        }

        public EspecificacaoProdutoDTO GetEspecificacaoProdutoByIndex(string codigoBarras, string codigoFornecedor, double valorCompra)
        {
            return _mapper.Map<EspecificacaoProdutoDTO>(_especificacaoProdutoRepository.Get(x => x.CodigoBarrasProduto == codigoBarras && x.CodigoFornecedorProduto == codigoFornecedor && x.ValorCompraProduto == valorCompra && x.EmEstoque));
        }

        public IEnumerable<EspecificacaoProdutoDTO> GetEspecificacaoProdutos(string codigoBarras, string codigoFornecedor)
        {
            return _mapper.Map<IEnumerable<EspecificacaoProdutoDTO>>(_especificacaoProdutoRepository.GetAll(x => x.CodigoBarrasProduto == codigoBarras && x.CodigoFornecedorProduto == codigoFornecedor && x.EmEstoque));
        }

        public EspecificacaoProdutoDTO Inserir(EspecificacaoProdutoDTO especificacaoParaInserir)
        {
            return _mapper.Map<EspecificacaoProdutoDTO>(_especificacaoProdutoRepository.CreateAsync(_mapper.Map<EspecificacaoProduto>(especificacaoParaInserir)).Result);
        }

        public EspecificacaoProdutoDTO Update(EspecificacaoProdutoDTO especificacaoParaInserir)
        {
            return _mapper.Map<EspecificacaoProdutoDTO>(_especificacaoProdutoRepository.UpdateAsync(_mapper.Map<EspecificacaoProduto>(especificacaoParaInserir)).Result);
        }

        public EspecificacaoProdutoDTO Delete(EspecificacaoProdutoDTO especificacaoParaDeletar)
        {
            return _mapper.Map<EspecificacaoProdutoDTO>(_especificacaoProdutoRepository.RemoveAsync(_mapper.Map<EspecificacaoProduto>(especificacaoParaDeletar)).Result);
        }
    }
}

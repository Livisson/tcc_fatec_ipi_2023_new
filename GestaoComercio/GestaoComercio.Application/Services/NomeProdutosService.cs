using AutoMapper;
using GestaoComercio.Application.Models.Fornecedor.Commands;
using GestaoComercio.Application.Models.NomeProdutos.Commands;
using GestaoComercio.Application.Models.Pedido.Commands;
using GestaoComercio.Domain.Entities;
using GestaoComercio.Domain.Interfaces;
using GestaoComercio.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Application.Services
{
    public class NomeProdutosService
    {

        private readonly IGenericRepository<NomeProdutos> _nomeProdutosRepository;
        private readonly IGenericRepository<Produto> _produtoRepository;
        private readonly IMapper _mapper;

        public NomeProdutosService(IGenericRepository<NomeProdutos> nomeProdutosRepository, IMapper mapper, IGenericRepository<Produto> produtoRepository)
        {
            _nomeProdutosRepository = nomeProdutosRepository;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }
        public async Task<NomeProdutosDTO> InserirNomeProduto(PostNomeProdutosCommand request)
        {
            var nomeProdutos = await _nomeProdutosRepository.GetAsync();
            foreach (var item in nomeProdutos)
            {
                char[] nomeDbChar = item.NomeProduto.Trim().ToLower().Normalize(NormalizationForm.FormD)
                    .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                    .ToArray();

                string nomeDb = new string(nomeDbChar);

                char[] nomeParaInserirChar = request.NomeProduto.Trim().ToLower().Normalize(NormalizationForm.FormD)
                    .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                    .ToArray();

                string nomeParaInserir = new string(nomeParaInserirChar);

                if (nomeParaInserir.Trim() == nomeDb.Trim())
                {
                    throw new MyExceptionApi("Não é possivel cadastrar Nome de produtos iguais!", HttpStatusCode.BadRequest);
                }

            }
            return _mapper.Map<NomeProdutosDTO>(await _nomeProdutosRepository.CreateAsync(_mapper.Map<NomeProdutos>(request)));
        }

        //public async Task<NomeProdutosDTO> AtualizarNomeProduto(PostNomeProdutosCommand request)
        //{
        //    var nomeProdutos = await _nomeProdutosRepository.GetAsync();
        //    foreach (var item in nomeProdutos)
        //    {
        //        if (item.Id != request.Id)
        //        {
        //            char[] nomeDbChar = item.NomeProduto.Trim().ToLower().Normalize(NormalizationForm.FormD)
        //                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
        //                .ToArray();

        //            string nomeDb = new string(nomeDbChar);

        //            char[] nomeParaInserirChar = request.NomeProduto.Trim().ToLower().Normalize(NormalizationForm.FormD)
        //                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
        //                .ToArray();

        //            string nomeParaInserir = new string(nomeParaInserirChar);

        //            if (nomeParaInserir.Trim() == nomeDb.Trim())
        //            {
        //                throw new MyExceptionApi("Não é possivel cadastrar Nome de produtos iguais!", HttpStatusCode.BadRequest);
        //            }
        //        }
        //    }
        //    return _mapper.Map<NomeProdutosDTO>(await _nomeProdutosRepository.UpdateAsync(_mapper.Map<NomeProdutos>(request)));
        //}
        public async Task<ProdutoDTO> AtualizarNomeProduto(PostNomeProdutosCommand request)
        {
            var produto = _produtoRepository.Get(x => x.FornecedorCpnj == request.CnpjFornecedor && x.CodigoBarras == request.CodigoBarras);
            produto.Nome = request.NomeProduto;
            produto.Fornecedor = null;
            produto.EspecificacoesDeProduto = null;
            produto.PedidosDeProduto = null;
            produto.ProdutosVenda = null;
            return _mapper.Map<ProdutoDTO>(await _produtoRepository.UpdateAsync(_mapper.Map<Produto>(produto)));
        }

        public async Task<NomeProdutosDTO> DeletarNomeProduto(int id)
        {
            var nomeProduto = _nomeProdutosRepository.Get(x => x.Id == id);
            return _mapper.Map<NomeProdutosDTO>(await _nomeProdutosRepository.RemoveAsync(_mapper.Map<NomeProdutos>(nomeProduto)));
        }

        public async Task<IEnumerable<NomeProdutosDTO>> ConsultaNomeProdutos()
        {
            return _mapper.Map<IEnumerable<NomeProdutosDTO>>(await _nomeProdutosRepository.GetAsync());
        }
    }
}

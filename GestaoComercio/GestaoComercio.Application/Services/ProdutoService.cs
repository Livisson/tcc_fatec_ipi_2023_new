using AutoMapper;
using GestaoComercio.Application.Models.NomeProdutos.Commands;
using GestaoComercio.Application.Models.Pedido.Commands;
using GestaoComercio.Application.Models.Precificacao.Commands;
using GestaoComercio.Application.Models.Responses;
using GestaoComercio.Application.Responses;
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
    public class ProdutoService
    {
        private readonly IGenericRepository<Produto> _produtoRepository;
        private readonly EspecificacoesProdutoService _especificacoesProdutoService;
        private readonly IMapper _mapper;

        public ProdutoService(IGenericRepository<Produto> produtoRepository, EspecificacoesProdutoService especificacoesProdutoService, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _especificacoesProdutoService = especificacoesProdutoService;
            _mapper = mapper;
        }

        public ProdutoDTO GetProdutoByIndex(string codigoBarras, string codigoFornecedor)
        {
            return _mapper.Map<ProdutoDTO>(_produtoRepository.Get(x => x.CodigoBarras == codigoBarras && x.FornecedorCpnj == codigoFornecedor));
        }

        public ProdutoDTO GetProdutoByCodigoBarras(string codigoBarras)
        {
            return  _mapper.Map<ProdutoDTO>(_produtoRepository.Get(x => x.CodigoBarras == codigoBarras));
        }

        public ProdutoDTO GetProdutoByName(string codigoBarras, string nome)
        {
            return _mapper.Map<ProdutoDTO>(_produtoRepository.Get(x => x.CodigoBarras == codigoBarras && x.Nome == nome));
        }

        public async Task<IEnumerable<ProdutoDTO>> GetProdutos()
        {
            return _mapper.Map<IEnumerable<ProdutoDTO>>(await _produtoRepository.GetAsync());
        }

        public List<TelaPrecificacaoResponse> ConsultaPrecificacao(string codigoFornecedor)
        {
            //if (codigoFornecedor == "teste")
            //{
            //    throw new Exception("Se fudeu");
            //}
            //else
            //{
            //    throw new MyExceptionApi("Exception API", HttpStatusCode.BadRequest);
            //}

            var produtos = new List<Produto>();
            if (codigoFornecedor == "" || codigoFornecedor == null || codigoFornecedor == "undefined")
            {
                produtos = _produtoRepository.GetAsync().Result.ToList();
            }
            else
            {
                produtos = _produtoRepository.GetAll(x => x.FornecedorCpnj == codigoFornecedor).ToList();
            }

            //var produtos = _produtoRepository.GetAll(x => x.FornecedorCpnj == codigoFornecedor);
            List<TelaPrecificacaoResponse> list = new List<TelaPrecificacaoResponse>();

            foreach (var item in produtos)
            {
                var produtoEspecificacao = _mapper.Map<IEnumerable<EspecificacaoProdutoDTO>>(_especificacoesProdutoService.GetEspecificacaoProdutos(item.CodigoBarras, item.FornecedorCpnj));

                if (produtoEspecificacao.Count() > 0)
                {
                    TelaPrecificacaoResponse registro = new TelaPrecificacaoResponse
                    {
                        CodigoBarras = item.CodigoBarras,
                        Estoque = item.QtdEstoqueTotal,
                        NomeProduto = item.Nome,
                        PerDesconto = item.PerDesconto,
                        PerMargem = item.PerMargem,
                        ValorCompra = produtoEspecificacao.OrderByDescending(x => x.ValorCompraProduto).FirstOrDefault().ValorCompraProduto,
                        ValorSugerido = item.ValorSugerido,
                        ValorVenda = item.ValorVenda,
                        CodigoFornecedor = item.FornecedorCpnj,
                        NomeFornecedor = item.Fornecedor.Nome
                    };

                    list.Add(registro);
                }

            }
            return list;
        }

        public List<TelaEstoqueResponse> ConsultaEstoque(string codigoFornecedor, string nomeProduto)
        {
            var produtos = new List<Produto>();
            if (codigoFornecedor == "" || codigoFornecedor == null || codigoFornecedor == "undefined")
            {
                produtos = _produtoRepository.GetAsync().Result.ToList();
            }
            else
            {
                produtos = _produtoRepository.GetAll(x => x.FornecedorCpnj == codigoFornecedor).ToList();
            }

            if (nomeProduto != "" && nomeProduto != null && nomeProduto != "undefined")
            {
                produtos = produtos.Where(x => x.Nome == nomeProduto).ToList();
            }

            List<TelaEstoqueResponse> list = new List<TelaEstoqueResponse>();

            foreach (var item in produtos)
            {
                //var produtoEspecificacao = _mapper.Map<IEnumerable<EspecificacaoProdutoDTO>>(_especificacoesProdutoService.GetEspecificacaoProdutos(item.CodigoBarras, item.FornecedorCpnj));

                TelaEstoqueResponse registro = new TelaEstoqueResponse
                {
                    NomeFornecedor = item.Fornecedor.Nome,
                    CnpjFornecedor = item.Fornecedor.Cnpj,
                    NomeProduto = item.Nome,
                    CodigoBarras = item.CodigoBarras,
                    Quantidade = item.QtdEstoqueTotal
                };

                list.Add(registro);
            }
            return list;
        }
        public ProdutoDTO Inserir(ProdutoDTO produtoParaInserir)
        {
            return _mapper.Map<ProdutoDTO>(_produtoRepository.CreateAsync(_mapper.Map<Produto>(produtoParaInserir)).Result);
        }

        public ProdutoDTO Update(ProdutoDTO produtoParaInserir)
        {
            var entity = _produtoRepository.Get(x => x.CodigoBarras == produtoParaInserir.CodigoBarras && x.FornecedorCpnj == produtoParaInserir.FornecedorCpnj);

            _mapper.Map(produtoParaInserir, entity);

            entity.Fornecedor = null;
            //entity.EspecificacoesDeProduto = null;
            //entity.PedidosDeProduto = null;
            //entity.ProdutosVenda = null;
            //return _mapper.Map<ProdutoDTO>(entity);
            return _mapper.Map<ProdutoDTO>(_produtoRepository.UpdateAsync(_mapper.Map<Produto>(entity)).Result);
        }

        public ProdutoDTO UpdateValoresVenda(PostPrecificacaoCommand produtoParaAtualizar)
        {
            var entity = _produtoRepository.Get(x => x.CodigoBarras == produtoParaAtualizar.CodigoBarras && x.FornecedorCpnj == produtoParaAtualizar.CodigoFornecedor);

            entity.PerMargem = produtoParaAtualizar.PerMargem;
            entity.PerDesconto = produtoParaAtualizar.PerDesconto;
            entity.ValorVenda = produtoParaAtualizar.ValorVenda;

            //return _mapper.Map<ProdutoDTO>(entity);
            entity.Fornecedor = null;
            entity.EspecificacoesDeProduto = null;
            return _mapper.Map<ProdutoDTO>(_produtoRepository.UpdateAsync(entity).Result);
        }

        public async Task<ProdutoDTO> DeletarProduto(ProdutoDTO request)
        {
            return _mapper.Map<ProdutoDTO>(await _produtoRepository.RemoveAsync(_mapper.Map<Produto>(request)));
        }

    }
}

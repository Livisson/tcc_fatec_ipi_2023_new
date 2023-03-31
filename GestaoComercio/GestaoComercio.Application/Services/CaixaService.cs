using AutoMapper;
using GestaoComercio.Application.Models.Caixa.Commands;
using GestaoComercio.Application.Models.Fornecedor.Commands;
using GestaoComercio.Application.Models.Pedido.Commands;
using GestaoComercio.Domain.Entities;
using GestaoComercio.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using GestaoComercio.Application.Models.Responses;

namespace GestaoComercio.Application.Services
{
    public class CaixaService
    {

        private readonly IGenericRepository<Produto> _produtoRepository;
        private readonly IGenericRepository<Despesa> _despesaRepository;
        private readonly IGenericRepository<DespesaHistorico> _despesaHistoricoRepository;
        private readonly IGenericRepository<ProdutosVenda> _produtosVendaRepository;
        private readonly EspecificacoesProdutoService _especificacoesProdutoService;
        private readonly IGenericRepository<Caixa> _caixaRepository;
        private readonly ProdutoService _produtoService;
        private readonly IMapper _mapper;

        public CaixaService(ProdutoService produtoService, IGenericRepository<Produto> produtoRepository, IGenericRepository<ProdutosVenda> produtosVendaRepository, IGenericRepository<Caixa> caixaRepository, EspecificacoesProdutoService especificacoesProdutoService, IGenericRepository<Despesa> despesaRepository, IMapper mapper, IGenericRepository<DespesaHistorico> despesaHistoricoRepository)
        {
            _produtoService = produtoService;
            _produtoRepository = produtoRepository;
            _produtosVendaRepository = produtosVendaRepository;
            _especificacoesProdutoService = especificacoesProdutoService;
            _caixaRepository = caixaRepository;
            _despesaRepository = despesaRepository;
            _mapper = mapper;
            _despesaHistoricoRepository = despesaHistoricoRepository;
        }
        public async Task<IEnumerable<ProdutosVendaDTO>> InserirCompra(List<PostCaixaCommand> request)
        {

            double valorTotalVenda = 0;

            foreach (var item in request)
            {
                var produto = _produtoService.GetProdutoByName(item.CodigoBarras, item.NomeProduto);

                valorTotalVenda += (produto.ValorVenda * item.Quantidade);
            }

            var venda = new CaixaDTO
            {
                ValorVenda = valorTotalVenda,
                DataVenda = DateTime.Now
            };

            var registroCaixa = await _caixaRepository.CreateAsync(_mapper.Map<Caixa>(venda));

            foreach (var item2 in request)
            {
                var produto = _produtoService.GetProdutoByName(item2.CodigoBarras, item2.NomeProduto);
                var produtoEspecificacao = _especificacoesProdutoService.GetEspecificacaoProdutos(produto.CodigoBarras, produto.FornecedorCpnj).OrderByDescending(x => x.ValorCompraProduto);

                //Atualizar quantidade do estoque
                produto.QtdEstoqueTotal = produto.QtdEstoqueTotal - item2.Quantidade;
                _produtoService.Update(produto);

                int qtdComprada = item2.Quantidade;
                foreach (var estoque in produtoEspecificacao)
                {
                    if ((estoque.QtdEstoque - qtdComprada) < 0)
                    {
                        //var especificaoParaAtualizar = _especificacoesProdutoService.GetEspecificacaoProdutoByIndex(estoque.CodigoBarrasProduto, estoque.CodigoFornecedorProduto, estoque.ValorCompraProduto);
                        qtdComprada = qtdComprada - estoque.QtdEstoque;
                        estoque.QtdEstoque = 0;
                        estoque.EmEstoque = false;
                        
                        _especificacoesProdutoService.Update(estoque);
                    }
                    else
                    {
                        //var especificaoParaAtualizar = _especificacoesProdutoService.GetEspecificacaoProdutoByIndex(estoque.CodigoBarrasProduto, estoque.CodigoFornecedorProduto, estoque.ValorCompraProduto);

                        estoque.QtdEstoque = estoque.QtdEstoque - qtdComprada;
                        if (estoque.QtdEstoque == 0)
                        {
                            estoque.EmEstoque = false;
                        }
                        _especificacoesProdutoService.Update(estoque);
                        break;
                    }
                }
                

                 //Inserir na tabela de venda
                 var produtoVenda = new ProdutosVendaDTO
                {
                    CodigoBarrasProduto = produto.CodigoBarras,
                    CodigoFornecedorProduto = produto.FornecedorCpnj,
                    ValorVendaProduto = produto.ValorVenda,
                    CaixaId = registroCaixa.Id,
                    Quantidade = item2.Quantidade,
                    DataVenda = registroCaixa.DataVenda,
                    Lucro = produto.ValorVenda - (produto.ValorVenda / (1 - produto.PerDesconto + produto.PerMargem - (produto.PerMargem * produto.PerDesconto)))
                };

                await _produtosVendaRepository.CreateAsync(_mapper.Map<ProdutosVenda>(produtoVenda));
            }

            return _mapper.Map<IEnumerable<ProdutosVendaDTO>>(await _produtosVendaRepository.GetAsync());
        }

        public List<TelaConsolidadoResponse> ConsultarConsolidadoMes(string mesAno)
        {
            //JArray json = new JArray();
            var json = new List<TelaConsolidadoResponse>();

            int year = Int32.Parse(mesAno.ToString().Substring(0, 4));
            int month = Int32.Parse(mesAno.ToString().Substring(4, 2));
            
            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                if (month == DateTime.Now.Month)//Se o mes for mes atual da pesquisa
                {
                    if (i > DateTime.Now.Day)//Se o dia for maior que hoje
                    {
                        var vendas = 10; //media diaria de vendas
                        var despesas = _despesaHistoricoRepository.GetAll(x => x.DiaVencimento == i).ToList().Count == 0 ? 0 : _despesaHistoricoRepository.GetAll(x => x.DiaVencimento == i).ToList().Sum(x => x.Valor);
                        var resumo = vendas - despesas;

                        string appObj = "{'Data':'"+ new DateTime(year, month, i, 0, 0, 0).ToString("dd/MM/yyyy") + "', 'Receitas':'" + vendas + "', 'Despesa':'" + despesas + "', 'Resumo':'" + resumo + "'}";

                        TelaConsolidadoResponse registroReceita = new TelaConsolidadoResponse
                        {
                            Title = "R R$" + vendas.ToString("N2"),
                            Start = new DateTime(year, month, i, 0, 0, 0),
                            End = new DateTime(year, month, i, 0, 0, 0),
                            Tipo = vendas >= 0 ? "positivo" : "negativo"
                        };
                        json.Add(registroReceita);

                        TelaConsolidadoResponse registroDespesa = new TelaConsolidadoResponse
                        {
                            Title = "D R$" + despesas.ToString("N2"),
                            Start = new DateTime(year, month, i, 0, 0, 0),
                            End = new DateTime(year, month, i, 0, 0, 0),
                            Tipo = "negativo"
                        };
                        json.Add(registroDespesa);

                        TelaConsolidadoResponse registroSaldo = new TelaConsolidadoResponse
                        {
                            Title = "S R$" + resumo.ToString("N2"),
                            Start = new DateTime(year, month, i, 0, 0, 0),
                            End = new DateTime(year, month, i, 0, 0, 0),
                            Tipo = resumo >= 0 ? "positivo" : "negativo"
                        };;
                        json.Add(registroSaldo);
                    }
                    else
                    {
                        var vendas = _caixaRepository.GetAll(x => x.DataVenda >= new DateTime(year, month, i, 0, 0, 0) && x.DataVenda <= new DateTime(year, month, i, 23, 59, 59)).ToList().Count == 0 ? 0 : _caixaRepository.GetAll(x => x.DataVenda >= new DateTime(year, month, i, 0, 0, 0) && x.DataVenda <= new DateTime(year, month, i, 23, 59, 59)).ToList().Sum(x => x.ValorVenda);
                        var despesas = _despesaHistoricoRepository.GetAll(x => x.DiaVencimento == i).ToList().Count == 0 ? 0 : _despesaHistoricoRepository.GetAll(x => x.DiaVencimento == i).ToList().Sum(x => x.Valor);
                        var resumo = vendas - despesas;

                        string appObj = "{'Data':'" + new DateTime(year, month, i, 0, 0, 0).ToString("dd/MM/yyyy") + "', 'Receitas':'" + vendas + "', 'Despesa':'" + despesas + "', 'Resumo':'" + resumo + "'}";

                        TelaConsolidadoResponse registroReceita = new TelaConsolidadoResponse
                        {
                            Title = "R R$" + vendas.ToString("N2"),
                            Start = new DateTime(year, month, i, 0, 0, 0),
                            End = new DateTime(year, month, i, 0, 0, 0),
                            Tipo = vendas >= 0 ? "positivo" : "negativo"
                        };
                        json.Add(registroReceita);

                        TelaConsolidadoResponse registroDespesa = new TelaConsolidadoResponse
                        {
                            Title = "D R$" + despesas.ToString("N2"),
                            Start = new DateTime(year, month, i, 0, 0, 0),
                            End = new DateTime(year, month, i, 0, 0, 0),
                            Tipo = "negativo"
                        };
                        json.Add(registroDespesa);

                        TelaConsolidadoResponse registroSaldo = new TelaConsolidadoResponse
                        {
                            Title = "S R$" + resumo.ToString("N2"),
                            Start = new DateTime(year, month, i, 0, 0, 0),
                            End = new DateTime(year, month, i, 0, 0, 0),
                            Tipo = resumo >= 0 ? "positivo" : "negativo"
                        }; ;
                        json.Add(registroSaldo);
                    }
                }
                else
                {
                    var vendas = _caixaRepository.GetAll(x => x.DataVenda >= new DateTime(year, month, i, 0, 0, 0) && x.DataVenda <= new DateTime(year, month, i, 23, 59, 59)).ToList().Count == 0 ? 0 : _caixaRepository.GetAll(x => x.DataVenda >= new DateTime(year, month, i, 0, 0, 0) && x.DataVenda <= new DateTime(year, month, i, 23, 59, 59)).ToList().Sum(x => x.ValorVenda);
                    var despesas = _despesaHistoricoRepository.GetAll(x => x.DiaVencimento == i).ToList().Count == 0 ? 0 : _despesaHistoricoRepository.GetAll(x => x.DiaVencimento == i).ToList().Sum(x => x.Valor);
                    var resumo = vendas - despesas;

                    string appObj = "{'Data':'" + new DateTime(year, month, i, 0, 0, 0).ToString("dd/MM/yyyy") + "', 'Receitas':'" + vendas + "', 'Despesa':'" + despesas + "', 'Resumo':'" + resumo + "'}";

                    TelaConsolidadoResponse registroReceita = new TelaConsolidadoResponse
                    {
                        Title = "R R$" + vendas.ToString("N2"),
                        Start = new DateTime(year, month, i, 0, 0, 0),
                        End = new DateTime(year, month, i, 0, 0, 0),
                        Tipo = vendas >= 0 ? "positivo" : "negativo"
                    };
                    json.Add(registroReceita);

                    TelaConsolidadoResponse registroDespesa = new TelaConsolidadoResponse
                    {
                        Title = "D R$" + despesas.ToString("N2"),
                        Start = new DateTime(year, month, i, 0, 0, 0),
                        End = new DateTime(year, month, i, 0, 0, 0),
                        Tipo = "negativo"
                    };
                    json.Add(registroDespesa);

                    TelaConsolidadoResponse registroSaldo = new TelaConsolidadoResponse
                    {
                        Title = "S R$" + resumo.ToString("N2"),
                        Start = new DateTime(year, month, i, 0, 0, 0),
                        End = new DateTime(year, month, i, 0, 0, 0),
                        Tipo = resumo >= 0 ? "positivo" : "negativo"
                    }; ;
                    json.Add(registroSaldo);
                }
            }
            
            return json;
        }

    }
}

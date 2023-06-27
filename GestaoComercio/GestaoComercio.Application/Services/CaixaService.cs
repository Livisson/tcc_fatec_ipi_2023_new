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
using OpenAI_API;
using OpenAI_API.Completions;
using Newtonsoft.Json;

namespace GestaoComercio.Application.Services
{
    public class CaixaService
    {

        private readonly IGenericRepository<Produto> _produtoRepository;
        private readonly IGenericRepository<Despesa> _despesaRepository;
        private readonly IGenericRepository<DespesaHistorico> _despesaHistoricoRepository;
        private readonly IGenericRepository<ProdutosVenda> _produtosVendaRepository;
        private readonly IGenericRepository<Pedido> _pedidoRepository;
        private readonly EspecificacoesProdutoService _especificacoesProdutoService;
        private readonly IGenericRepository<Caixa> _caixaRepository;
        private readonly ProdutoService _produtoService;
        private readonly IMapper _mapper;

        public CaixaService(ProdutoService produtoService, IGenericRepository<Produto> produtoRepository, IGenericRepository<ProdutosVenda> produtosVendaRepository, IGenericRepository<Caixa> caixaRepository, EspecificacoesProdutoService especificacoesProdutoService, IGenericRepository<Despesa> despesaRepository, IMapper mapper, IGenericRepository<DespesaHistorico> despesaHistoricoRepository, IGenericRepository<Pedido> pedidoRepository)
        {
            _produtoService = produtoService;
            _produtoRepository = produtoRepository;
            _produtosVendaRepository = produtosVendaRepository;
            _especificacoesProdutoService = especificacoesProdutoService;
            _caixaRepository = caixaRepository;
            _despesaRepository = despesaRepository;
            _mapper = mapper;
            _despesaHistoricoRepository = despesaHistoricoRepository;
            _pedidoRepository = pedidoRepository;
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

            DateTime dataInicial = new DateTime(year, month, 1, 0, 0, 0);
            DateTime dataFinal = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);

            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                if (month == DateTime.Now.Month)//Se o mes for mes atual da pesquisa
                {
                    if (i > DateTime.Now.Day)//Se o dia for maior que hoje
                    {
                        var vendas = _caixaRepository.GetAll(x => x.DataVenda >= dataInicial.AddMonths(-3) && x.DataVenda <= dataFinal && x.DataVenda.Day == i).ToList().Count == 0 ? 0 : _caixaRepository.GetAll(x => x.DataVenda >= dataInicial.AddMonths(-3) && x.DataVenda <= dataFinal && x.DataVenda.Day == i).ToList().Sum(x => x.ValorVenda) / 3; //media diaria de vendas
                        var despesas = _despesaRepository.GetAll(x => x.DiaVencimento == i).ToList().Count == 0 ? 0 : _despesaRepository.GetAll(x => x.DiaVencimento == i).ToList().Sum(x => x.Valor);
                        var pedidos = _pedidoRepository.GetAll(x => x.DataVencimento >= new DateTime(year, month, i, 0, 0, 0) && x.DataVencimento <= new DateTime(year, month, i, 23, 59, 59));
                        var somaPorItem = pedidos.Select(pedido => new { Item = pedido.CodigoBarrasProduto, Soma = pedido.ValorCompra * pedido.Quantidade })
                            .GroupBy(p => p.Item)
                            .Select(g => new { Item = g.Key, Soma = g.Sum(p => p.Soma) });
                        var valorSomaPedidos = somaPorItem.Count() == 0 ? 0 : somaPorItem.Sum(x => x.Soma);
                        despesas = despesas + valorSomaPedidos;
                        var resumo = vendas - despesas;

                        string appObj = "{'Data':'"+ new DateTime(year, month, i, 0, 0, 0).ToString("dd/MM/yyyy") + "', 'Receitas':'" + vendas + "', 'Despesa':'" + despesas + "', 'Resumo':'" + resumo + "'}";

                        TelaConsolidadoResponse registroReceita = new TelaConsolidadoResponse
                        {
                            Title = "R R$" + vendas.ToString("N2"),
                            Start = new DateTime(year, month, i, 0, 0, 0),
                            End = new DateTime(year, month, i, 0, 0, 0),
                            Tipo = vendas >= 0 ? "positivoFuturo" : "negativo"
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
                            Tipo = resumo >= 0 ? "positivoFuturo" : "negativo"
                        };;
                        json.Add(registroSaldo);
                    }
                    else
                    {
                        var vendas = _caixaRepository.GetAll(x => x.DataVenda >= new DateTime(year, month, i, 0, 0, 0) && x.DataVenda <= new DateTime(year, month, i, 23, 59, 59)).ToList().Count == 0 ? 0 : _caixaRepository.GetAll(x => x.DataVenda >= new DateTime(year, month, i, 0, 0, 0) && x.DataVenda <= new DateTime(year, month, i, 23, 59, 59)).ToList().Sum(x => x.ValorVenda);
                        var despesas = _despesaRepository.GetAll(x => x.DiaVencimento == i).ToList().Count == 0 ? 0 : _despesaRepository.GetAll(x => x.DiaVencimento == i).ToList().Sum(x => x.Valor);
                        var pedidos = _pedidoRepository.GetAll(x => x.DataVencimento >= new DateTime(year, month, i, 0, 0, 0) && x.DataVencimento <= new DateTime(year, month, i, 23, 59, 59));
                        var somaPorItem = pedidos.Select(pedido => new { Item = pedido.CodigoBarrasProduto, Soma = pedido.ValorCompra * pedido.Quantidade })
                            .GroupBy(p => p.Item)
                            .Select(g => new { Item = g.Key, Soma = g.Sum(p => p.Soma) });
                        var valorSomaPedidos = somaPorItem.Count() == 0 ? 0 : somaPorItem.Sum(x => x.Soma);
                        despesas = despesas + valorSomaPedidos;
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
                else if (month > DateTime.Now.Month)
                {
                    var vendas = _caixaRepository.GetAll(x => x.DataVenda >= dataInicial.AddMonths(-3) && x.DataVenda <= dataFinal && x.DataVenda.Day == i).ToList().Count == 0 ? 0 : _caixaRepository.GetAll(x => x.DataVenda >= dataInicial.AddMonths(-3) && x.DataVenda <= dataFinal && x.DataVenda.Day == i).ToList().Sum(x => x.ValorVenda) / 3; //media diaria de vendas
                    var despesas = _despesaHistoricoRepository.GetAll(x => x.DataHistorico >= dataInicial && x.DataHistorico <= dataFinal && x.DiaVencimento == i).ToList().Count == 0 ? 0 : _despesaHistoricoRepository.GetAll(x => x.DataHistorico >= dataInicial && x.DataHistorico <= dataFinal && x.DiaVencimento == i).ToList().Sum(x => x.Valor);
                    var pedidos = _pedidoRepository.GetAll(x => x.DataVencimento >= new DateTime(year, month, i, 0, 0, 0) && x.DataVencimento <= new DateTime(year, month, i, 23, 59, 59));
                    var somaPorItem = pedidos.Select(pedido => new { Item = pedido.CodigoBarrasProduto, Soma = pedido.ValorCompra * pedido.Quantidade })
                        .GroupBy(p => p.Item)
                        .Select(g => new { Item = g.Key, Soma = g.Sum(p => p.Soma) });
                    var valorSomaPedidos = somaPorItem.Count() == 0 ? 0 : somaPorItem.Sum(x => x.Soma);
                    despesas = despesas + valorSomaPedidos;
                    var resumo = vendas - despesas;

                    string appObj = "{'Data':'" + new DateTime(year, month, i, 0, 0, 0).ToString("dd/MM/yyyy") + "', 'Receitas':'" + vendas + "', 'Despesa':'" + despesas + "', 'Resumo':'" + resumo + "'}";

                    TelaConsolidadoResponse registroReceita = new TelaConsolidadoResponse
                    {
                        Title = "R R$" + vendas.ToString("N2"),
                        Start = new DateTime(year, month, i, 0, 0, 0),
                        End = new DateTime(year, month, i, 0, 0, 0),
                        Tipo = vendas >= 0 ? "positivoFuturo" : "negativo"
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
                        Tipo = resumo >= 0 ? "positivoFuturo" : "negativo"
                    }; ;
                    json.Add(registroSaldo);
                }
                else
                {
                    var vendas = _caixaRepository.GetAll(x => x.DataVenda >= new DateTime(year, month, i, 0, 0, 0) && x.DataVenda <= new DateTime(year, month, i, 23, 59, 59)).ToList().Count == 0 ? 0 : _caixaRepository.GetAll(x => x.DataVenda >= new DateTime(year, month, i, 0, 0, 0) && x.DataVenda <= new DateTime(year, month, i, 23, 59, 59)).ToList().Sum(x => x.ValorVenda);
                    var despesas = _despesaHistoricoRepository.GetAll(x => x.DataHistorico >= dataInicial && x.DataHistorico <= dataFinal && x.DiaVencimento == i).ToList().Count == 0 ? 0 : _despesaHistoricoRepository.GetAll(x => x.DataHistorico >= dataInicial && x.DataHistorico <= dataFinal && x.DiaVencimento == i).ToList().Sum(x => x.Valor);
                    var pedidos = _pedidoRepository.GetAll(x => x.DataVencimento >= new DateTime(year, month, i, 0, 0, 0) && x.DataVencimento <= new DateTime(year, month, i, 23, 59, 59));
                    var somaPorItem = pedidos.Select(pedido => new { Item = pedido.CodigoBarrasProduto, Soma = pedido.ValorCompra * pedido.Quantidade })
                        .GroupBy(p => p.Item)
                        .Select(g => new { Item = g.Key, Soma = g.Sum(p => p.Soma) });
                    var valorSomaPedidos = somaPorItem.Count() == 0 ? 0 : somaPorItem.Sum(x => x.Soma);
                    despesas = despesas + valorSomaPedidos;
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

        public TelaResumoConsolidadoResponse ConsultarResumoConsolidadoMes(string mesAno)
        {
            //JArray json = new JArray();
            var json = new TelaResumoConsolidadoResponse();
            double receitaJson = 0;
            double despesaJson = 0;
            double receitaPrevistaJson = 0;
            double saldoGeralJson = 0;

            int year = Int32.Parse(mesAno.ToString().Substring(0, 4));
            int month = Int32.Parse(mesAno.ToString().Substring(4, 2));

            DateTime dataInicial = new DateTime(year, month, 1, 0, 0, 0);
            DateTime dataFinal = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);

            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                if (month == DateTime.Now.Month)//Se o mes for mes atual da pesquisa
                {
                    if (i > DateTime.Now.Day)//Se o dia for maior que hoje
                    {
                        var vendas = _caixaRepository.GetAll(x => x.DataVenda >= dataInicial.AddMonths(-3) && x.DataVenda <= dataFinal && x.DataVenda.Day == i).ToList().Count == 0 ? 0 : _caixaRepository.GetAll(x => x.DataVenda >= dataInicial.AddMonths(-3) && x.DataVenda <= dataFinal && x.DataVenda.Day == i).ToList().Sum(x => x.ValorVenda) / 3; //media diaria de vendas
                        var despesas = _despesaRepository.GetAll(x => x.DiaVencimento == i).ToList().Count == 0 ? 0 : _despesaRepository.GetAll(x => x.DiaVencimento == i).ToList().Sum(x => x.Valor);
                        var pedidos = _pedidoRepository.GetAll(x => x.DataVencimento >= new DateTime(year, month, i, 0, 0, 0) && x.DataVencimento <= new DateTime(year, month, i, 23, 59, 59));
                        var somaPorItem = pedidos.Select(pedido => new { Item = pedido.CodigoBarrasProduto, Soma = pedido.ValorCompra * pedido.Quantidade })
                            .GroupBy(p => p.Item)
                            .Select(g => new { Item = g.Key, Soma = g.Sum(p => p.Soma) });
                        var valorSomaPedidos = somaPorItem.Count() == 0 ? 0 : somaPorItem.Sum(x => x.Soma);
                        despesas = despesas + valorSomaPedidos;
                        var resumo = vendas - despesas;

                        despesaJson = despesaJson + despesas;
                        receitaPrevistaJson = receitaPrevistaJson + vendas;
                    }
                    else
                    {
                        var vendas = _caixaRepository.GetAll(x => x.DataVenda >= new DateTime(year, month, i, 0, 0, 0) && x.DataVenda <= new DateTime(year, month, i, 23, 59, 59)).ToList().Count == 0 ? 0 : _caixaRepository.GetAll(x => x.DataVenda >= new DateTime(year, month, i, 0, 0, 0) && x.DataVenda <= new DateTime(year, month, i, 23, 59, 59)).ToList().Sum(x => x.ValorVenda);
                        //var despesas = _despesaHistoricoRepository.GetAll(x => x.DataHistorico >= dataInicial && x.DataHistorico <= dataFinal && x.DiaVencimento == i).ToList().Count == 0 ? 0 : _despesaHistoricoRepository.GetAll(x => x.DataHistorico >= dataInicial && x.DataHistorico <= dataFinal && x.DiaVencimento == i).ToList().Sum(x => x.Valor);
                        var despesas = _despesaRepository.GetAll(x => x.DiaVencimento == i).ToList().Count == 0 ? 0 : _despesaRepository.GetAll(x => x.DiaVencimento == i).ToList().Sum(x => x.Valor);
                        var pedidos = _pedidoRepository.GetAll(x => x.DataVencimento >= new DateTime(year, month, i, 0, 0, 0) && x.DataVencimento <= new DateTime(year, month, i, 23, 59, 59));
                        var somaPorItem = pedidos.Select(pedido => new { Item = pedido.CodigoBarrasProduto, Soma = pedido.ValorCompra * pedido.Quantidade })
                            .GroupBy(p => p.Item)
                            .Select(g => new { Item = g.Key, Soma = g.Sum(p => p.Soma) });
                        var valorSomaPedidos = somaPorItem.Count() == 0 ? 0 : somaPorItem.Sum(x => x.Soma);
                        despesas = despesas + valorSomaPedidos;
                        var resumo = vendas - despesas;


                        despesaJson = despesaJson + despesas;
                        receitaJson = receitaJson + vendas;

                    }
                }
                else if (month > DateTime.Now.Month)
                {
                    var vendas = _caixaRepository.GetAll(x => x.DataVenda >= dataInicial.AddMonths(-3) && x.DataVenda <= dataFinal && x.DataVenda.Day == i).ToList().Count == 0 ? 0 : _caixaRepository.GetAll(x => x.DataVenda >= dataInicial.AddMonths(-3) && x.DataVenda <= dataFinal && x.DataVenda.Day == i).ToList().Sum(x => x.ValorVenda) / 3; //media diaria de vendas
                    var despesas = _despesaHistoricoRepository.GetAll(x => x.DataHistorico >= dataInicial && x.DataHistorico <= dataFinal && x.DiaVencimento == i).ToList().Count == 0 ? 0 : _despesaHistoricoRepository.GetAll(x => x.DataHistorico >= dataInicial && x.DataHistorico <= dataFinal && x.DiaVencimento == i).ToList().Sum(x => x.Valor);
                    var pedidos = _pedidoRepository.GetAll(x => x.DataVencimento >= new DateTime(year, month, i, 0, 0, 0) && x.DataVencimento <= new DateTime(year, month, i, 23, 59, 59));
                    var somaPorItem = pedidos.Select(pedido => new { Item = pedido.CodigoBarrasProduto, Soma = pedido.ValorCompra * pedido.Quantidade })
                        .GroupBy(p => p.Item)
                        .Select(g => new { Item = g.Key, Soma = g.Sum(p => p.Soma) });
                    var valorSomaPedidos = somaPorItem.Count() == 0 ? 0 : somaPorItem.Sum(x => x.Soma);
                    despesas = despesas + valorSomaPedidos;
                    var resumo = vendas - despesas;

                    despesaJson = despesaJson + despesas;
                    receitaPrevistaJson = receitaPrevistaJson + vendas;

                }
                else
                {
                    var vendas = _caixaRepository.GetAll(x => x.DataVenda >= new DateTime(year, month, i, 0, 0, 0) && x.DataVenda <= new DateTime(year, month, i, 23, 59, 59)).ToList().Count == 0 ? 0 : _caixaRepository.GetAll(x => x.DataVenda >= new DateTime(year, month, i, 0, 0, 0) && x.DataVenda <= new DateTime(year, month, i, 23, 59, 59)).ToList().Sum(x => x.ValorVenda);
                    var despesas = _despesaHistoricoRepository.GetAll(x => x.DataHistorico >= dataInicial && x.DataHistorico <= dataFinal && x.DiaVencimento == i).ToList().Count == 0 ? 0 : _despesaHistoricoRepository.GetAll(x => x.DataHistorico >= dataInicial && x.DataHistorico <= dataFinal && x.DiaVencimento == i).ToList().Sum(x => x.Valor);
                    var pedidos = _pedidoRepository.GetAll(x => x.DataVencimento >= new DateTime(year, month, i, 0, 0, 0) && x.DataVencimento <= new DateTime(year, month, i, 23, 59, 59));
                    var somaPorItem = pedidos.Select(pedido => new { Item = pedido.CodigoBarrasProduto, Soma = pedido.ValorCompra * pedido.Quantidade })
                        .GroupBy(p => p.Item)
                        .Select(g => new { Item = g.Key, Soma = g.Sum(p => p.Soma) });
                    var valorSomaPedidos = somaPorItem.Count() == 0 ? 0 : somaPorItem.Sum(x => x.Soma);
                    despesas = despesas + valorSomaPedidos;
                    var resumo = vendas - despesas;


                    despesaJson = despesaJson + despesas;
                    receitaJson = receitaJson + vendas;
                    
                }
            }

            json.Receita = "R$" + receitaJson.ToString("N2");
            json.ReceitaPrevista = "R$" + receitaPrevistaJson.ToString("N2");
            json.Despesa = "R$" + despesaJson.ToString("N2");
            json.SaldoGeral = "R$" + (receitaJson - despesaJson).ToString("N2");

            return json;
        }

        public async Task<string> UseChatGPT(List<TelaConsolidadoResponse> query)
        {
            string OutPutResult = "";

            string financialData = @"[ { ""Data"": ""01/03/2023"", ""Receita"": ""R$ 35.845,00"", ""Despesa"": ""R$ 10.578,00"", ""Saldo"": ""R$ 25.267,00"" }, { ""Data"": ""02/03/2023"", ""Receita"": ""R$ 31.488,00"", ""Despesa"": ""R$ 15.897,00"", ""Saldo"": ""R$ 15.591,00"" }, { ""Data"": ""03/03/2023"", ""Receita"": ""R$ 4.566,00"", ""Despesa"": ""R$ 8.475,00"", ""Saldo"": ""-R$ 3.909,00"" }, { ""Data"": ""04/03/2023"", ""Receita"": ""R$ 3.547,00"", ""Despesa"": ""R$ 6.247,00"", ""Saldo"": ""-R$ 2.700,00"" }, { ""Data"": ""05/03/2023"", ""Receita"": ""R$ 22.045,00"", ""Despesa"": ""R$ 62.541,00"", ""Saldo"": ""-R$ 40.496,00"" }, { ""Data"": ""06/03/2023"", ""Receita"": ""R$ 20.845,00"", ""Despesa"": ""R$ 5.998,00"", ""Saldo"": ""R$ 14.847,00"" }, { ""Data"": ""07/03/2023"", ""Receita"": ""R$ 18.425,00"", ""Despesa"": ""R$ 7.847,00"", ""Saldo"": ""R$ 10.578,00"" }, { ""Data"": ""08/03/2023"", ""Receita"": ""R$ 15.875,00"", ""Despesa"": ""R$ 6.874,00"", ""Saldo"": ""R$ 9.001,00"" }, { ""Data"": ""09/03/2023"", ""Receita"": ""R$ 14.885,00"", ""Despesa"": ""R$ 7.254,00"", ""Saldo"": ""R$ 7.631,00"" }, { ""Data"": ""10/03/2023"", ""Receita"": ""R$ 15.254,00"", ""Despesa"": ""R$ 49.854,00"", ""Saldo"": ""-R$ 34.600,00"" }, { ""Data"": ""11/03/2023"", ""Receita"": ""R$ 12.875,00"", ""Despesa"": ""R$ 6.874,00"", ""Saldo"": ""R$ 6.001,00"" }, { ""Data"": ""12/03/2023"", ""Receita"": ""R$ 10.567,00"", ""Despesa"": ""R$ 6.578,00"", ""Saldo"": ""R$ 3.989,00"" }, { ""Data"": ""13/03/2023"", ""Receita"": ""R$ 9.245,00"", ""Despesa"": ""R$ 5.687,00"", ""Saldo"": ""R$ 3.558,00"" }, { ""Data"": ""14/03/2023"", ""Receita"": ""R$ 7.985,00"", ""Despesa"": ""R$ 5.897,00"", ""Saldo"": ""R$ 2.088,00"" }, { ""Data"": ""15/03/2023"", ""Receita"": ""R$ 6.878,00"", ""Despesa"": ""R$ 8.974,00"", ""Saldo"": ""-R$ 2.096,00"" }, { ""Data"": ""16/03/2023"", ""Receita"": ""R$ 6.810,00"", ""Despesa"": ""R$ 6.248,00"", ""Saldo"": ""R$ 562,00"" }, { ""Data"": ""17/03/2023"", ""Receita"": ""R$ 5.674,00"", ""Despesa"": ""R$ 18.687,00"", ""Saldo"": ""-R$ 13.013,00"" }, { ""Data"": ""18/03/2023"", ""Receita"": ""R$ 4.582,00"", ""Despesa"": ""R$ 12.584,00"", ""Saldo"": ""-R$ 8.002,00"" }, { ""Data"": ""19/03/2023"", ""Receita"": ""R$ 4.695,00"", ""Despesa"": ""R$ 7.984,00"", ""Saldo"": ""-R$ 3.289,00"" }, { ""Data"": ""20/03/2023"", ""Receita"": ""R$ 12.582,00"", ""Despesa"": ""R$ 28.422,00"", ""Saldo"": ""-R$ 15.840,00"" }, { ""Data"": ""21/03/2023"", ""Receita"": ""R$ 11.598,00"", ""Despesa"": ""R$ 8.974,00"", ""Saldo"": ""R$ 2.624,00"" }, { ""Data"": ""22/03/2023"", ""Receita"": ""R$ 10.598,00"", ""Despesa"": ""R$ 9.874,00"", ""Saldo"": ""R$ 724,00"" }, { ""Data"": ""23/03/2023"", ""Receita"": ""R$ 8.245,00"", ""Despesa"": ""R$ 7.984,00"", ""Saldo"": ""R$ 261,00"" }, { ""Data"": ""24/03/2023"", ""Receita"": ""R$ 7.899,00"", ""Despesa"": ""R$ 6.485,00"", ""Saldo"": ""R$ 1.414,00"" }, { ""Data"": ""25/03/2023"", ""Receita"": ""R$ 6.874,00"", ""Despesa"": ""R$ 5.684,00"", ""Saldo"": ""R$ 1.190,00"" }, { ""Data"": ""26/03/2023"", ""Receita"": ""R$ 5.682,00"", ""Despesa"": ""R$ 8.241,00"", ""Saldo"": ""-R$ 2.559,00"" }, { ""Data"": ""27/03/2023"", ""Receita"": ""R$ 4.687,00"", ""Despesa"": ""R$ 5.478,00"", ""Saldo"": ""-R$ 791,00"" }, { ""Data"": ""28/03/2023"", ""Receita"": ""R$ 4.987,00"", ""Despesa"": ""R$ 6.574,00"", ""Saldo"": ""-R$ 1.587,00"" }, { ""Data"": ""29/03/2023"", ""Receita"": ""R$ 6.894,00"", ""Despesa"": ""R$ 5.874,00"", ""Saldo"": ""R$ 1.020,00"" }, { ""Data"": ""30/03/2023"", ""Receita"": ""R$ 34.985,00"", ""Despesa"": ""R$ 11.578,00"", ""Saldo"": ""R$ 23.407,00"" }, { ""Data"": ""31/03/2023"", ""Receita"": ""R$ 26.584,00"", ""Despesa"": ""R$ 7.894,00"", ""Saldo"": ""R$ 18.690,00"" } ]";

            DateTime dataConsolidado = query.FirstOrDefault().Start;

            List<RequisicaoChatGPTConsolidado> dadosMensais = new List<RequisicaoChatGPTConsolidado>();
            for (int i = 1; i <= DateTime.DaysInMonth(dataConsolidado.Year, dataConsolidado.Month); i++)
            {
                var registros = query.FindAll(x => x.Start == new DateTime(dataConsolidado.Year, dataConsolidado.Month, i));
                RequisicaoChatGPTConsolidado dadoDiario = new RequisicaoChatGPTConsolidado();

                dadoDiario.Data = new DateTime(dataConsolidado.Year, dataConsolidado.Month, i).ToString().Substring(0, 10);
                foreach (var item in registros)
                {
                    if (item.Title.Substring(0, 1) == "R")
                    {
                        dadoDiario.Receita = item.Title.Remove(0, 2);
                    }
                    if (item.Title.Substring(0, 1) == "D")
                    {
                        dadoDiario.Despesa = item.Title.Remove(0, 2);
                    }
                    if (item.Title.Substring(0, 1) == "S")
                    {
                        dadoDiario.Saldo = item.Title.Remove(0, 2);
                    }
                }

                dadosMensais.Add(dadoDiario);
            }

            string json = JsonConvert.SerializeObject(dadosMensais);

            var openai = new OpenAIAPI("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");
            CompletionRequest completionRequest = new CompletionRequest();
            //completionRequest.Prompt = "Com base nos dados financeiros, faça um resumo estatístico" + "\n\nDados Financeiros: " + financialData;
            completionRequest.Prompt = "Com base nos dados financeiros, faça um resumo estatístico" + "\n\nDados Financeiros: " + json;
            completionRequest.Model = OpenAI_API.Models.Model.DavinciText;
            completionRequest.MaxTokens = 1000;

            var completions = openai.Completions.CreateCompletionAsync(completionRequest);

            foreach (var completion in completions.Result.Completions)
            {
                OutPutResult += completion.Text;
            }

            return OutPutResult;
        }
    }
}

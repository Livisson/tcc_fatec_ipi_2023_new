using AutoMapper;
using GestaoComercio.Application.Models.Caixa.Commands;
using GestaoComercio.Application.Services;
using GestaoComercio.Domain.Entities;
using GestaoComercio.Domain.Interfaces;
using GestaoComercio.WebUI.Models.Caixa.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//using System.Web.Http;

namespace GestaoComercio.WebUI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CaixaController : ControllerBase
    {
        private readonly ILogger<CaixaController> _logger;
        private readonly PedidoService _pedidoService;
        private readonly CaixaService _caixaService;
        private readonly IMapper _mapper;
        private readonly ProdutoService _produtoService;
        private readonly EspecificacoesProdutoService _especificacoesProdutoService;
        private readonly IGenericRepository<EspecificacaoProduto> _especificacaoProdutoRepository;
        private readonly IGenericRepository<Pedido> _pedidoRepository;
        private readonly IGenericRepository<Produto> _produtoRepository;
        private readonly IGenericRepository<ProdutosVenda> _produtosVendaRepository;
        private readonly IGenericRepository<Fornecedor> _fornecedorRepository;
        private readonly IGenericRepository<Caixa> _caixaRepository;
        private readonly IGenericRepository<Despesa> _despesaRepository;
        private readonly IGenericRepository<DespesaHistorico> _despesaHistoricoRepository;

        public CaixaController(ILogger<CaixaController> logger, IMapper mapper, IGenericRepository<Pedido> pedidoRepository, IGenericRepository<Produto> produtoRepository, IGenericRepository<EspecificacaoProduto> especificacaoProdutoRepository, IGenericRepository<ProdutosVenda> produtosVendaRepository, IGenericRepository<Caixa> caixaRepository, IGenericRepository<Despesa> despesaRepository, IGenericRepository<DespesaHistorico> despesaHistoricoRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _especificacoesProdutoService = new EspecificacoesProdutoService(especificacaoProdutoRepository, mapper);
            _produtoService = new ProdutoService(produtoRepository, _especificacoesProdutoService, mapper);
            _caixaService = new CaixaService(_produtoService, produtoRepository, produtosVendaRepository, caixaRepository, _especificacoesProdutoService, despesaRepository, mapper, despesaHistoricoRepository);
        }

        [HttpPost]
        public async Task<IActionResult> PostCaixa(List<PostCaixaModel> request) =>
            Ok(await _caixaService.InserirCompra(_mapper.Map<List<PostCaixaCommand>>(request)));

        [HttpGet]
        public IActionResult GetProdutoCaixa(string request) =>
            Ok(_produtoService.GetProdutoByCodigoBarras(request));

        [HttpGet("getConsolidado")]
        public IActionResult GetConsolidado(string data) 
        {
                //request = 202303;
                var teste = _caixaService.ConsultarConsolidadoMes(data);
                return Ok(teste);
        }

    }
}

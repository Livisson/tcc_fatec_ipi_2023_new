using AutoMapper;
using GestaoComercio.Application.Models.Pedido.Commands;
using GestaoComercio.Application.Services;
using GestaoComercio.Domain.Entities;
using GestaoComercio.Domain.Interfaces;
using GestaoComercio.WebUI.Models;
using GestaoComercio.WebUI.Models.NomeProdutos.Commands;
using GestaoComercio.WebUI.Models.Pedido.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoComercio.WebUI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly ILogger<PedidoController> _logger;
        private readonly PedidoService _pedidoService;
        private readonly IMapper _mapper;
        private readonly ProdutoService _produtoService;
        private readonly EspecificacoesProdutoService _especificacoesProdutoService;
        private readonly FornecedorService _fornecedorService;
        private readonly IGenericRepository<EspecificacaoProduto> _especificacaoProdutoRepository;
        private readonly IGenericRepository<Pedido> _pedidoRepository;
        private readonly IGenericRepository<Produto> _produtoRepository;

        public PedidoController(ILogger<PedidoController> logger, IMapper mapper, IGenericRepository<Pedido> pedidoRepository, IGenericRepository<Produto> produtoRepository, IGenericRepository<EspecificacaoProduto> especificacaoProdutoRepository, IGenericRepository<Fornecedor> fornecedorRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _especificacoesProdutoService = new EspecificacoesProdutoService(especificacaoProdutoRepository, mapper);
            _produtoService = new ProdutoService(produtoRepository, _especificacoesProdutoService, mapper);
            _pedidoService = new PedidoService(_produtoService, mapper, _especificacoesProdutoService, pedidoRepository, fornecedorRepository);
        }

        [HttpPost]
        //public async Task<IActionResult> PostPedido(PostPedidoModel request)
        //{
        //    await _pedidoService.InserirPedido(_mapper.Map<PostPedidoCommand>(request));
        //    return Ok();
        //}

        public async Task<IActionResult> PostPedido(PostPedidoModel request) =>
            Ok(await _pedidoService.InserirPedido(_mapper.Map<PostPedidoCommand>(request)));

        //[HttpGet("{fornecedor}")]
        //public IActionResult GetPedidos(string fornecedor) =>
        //    Ok(_pedidoService.ConsultaPedidos(fornecedor));

        [HttpGet]
        public IActionResult GetPedidos(string codigoFornecedor) =>
            Ok(_pedidoService.ConsultaPedidos(codigoFornecedor));

        [HttpPost("deletePedido")]
        public async Task<IActionResult> DeletePedido(PostPedidoModel request) =>
            Ok(await _pedidoService.DeletePedido(_mapper.Map<PostPedidoCommand>(request)));

        [HttpGet("getEstoque")]
        public IActionResult GetEstoque(string codigoFornecedor, string nomeProduto) =>
            Ok(_produtoService.ConsultaEstoque(codigoFornecedor, nomeProduto));

        [HttpGet("getProdutos")]
        public async Task<IActionResult> GetProdutos() =>
            Ok(await _produtoService.GetProdutos());

    }
}

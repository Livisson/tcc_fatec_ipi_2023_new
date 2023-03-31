using AutoMapper;
using GestaoComercio.Application.Models.Fornecedor.Commands;
using GestaoComercio.Application.Models.Pedido.Commands;
using GestaoComercio.Application.Models.Precificacao.Commands;
using GestaoComercio.Application.Services;
using GestaoComercio.Domain.Entities;
using GestaoComercio.Domain.Interfaces;
using GestaoComercio.WebUI.Models;
using GestaoComercio.WebUI.Models.Fornecedor.Commands;
using GestaoComercio.WebUI.Models.Pedido.Commands;
using GestaoComercio.WebUI.Models.Precificacao.Commands;
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
    public class FornecedorController : ControllerBase
    {
        private readonly ILogger<FornecedorController> _logger;
        private readonly PedidoService _pedidoService;
        private readonly FornecedorService _fornecedorService;
        private readonly IMapper _mapper;
        private readonly ProdutoService _produtoService;
        private readonly EspecificacoesProdutoService _especificacoesProdutoService;
        private readonly IGenericRepository<EspecificacaoProduto> _especificacaoProdutoRepository;
        private readonly IGenericRepository<Pedido> _pedidoRepository;
        private readonly IGenericRepository<Produto> _produtoRepository;
        private readonly IGenericRepository<Fornecedor> _fornecedorRepository;

        public FornecedorController(ILogger<FornecedorController> logger, IMapper mapper, IGenericRepository<Pedido> pedidoRepository, IGenericRepository<Produto> produtoRepository, IGenericRepository<EspecificacaoProduto> especificacaoProdutoRepository, IGenericRepository<Fornecedor> fornecedorRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _especificacoesProdutoService = new EspecificacoesProdutoService(especificacaoProdutoRepository, mapper);
            _produtoService = new ProdutoService(produtoRepository, _especificacoesProdutoService, mapper);
            _fornecedorService = new FornecedorService(fornecedorRepository, mapper, pedidoRepository);
        }

        [HttpPost]
        public async Task<IActionResult> PostFornecedor(PostFornecedorModel request) =>
            Ok(await _fornecedorService.InserirFornecedor(_mapper.Map<PostFornecedorCommand>(request)));

        [HttpPut]
        public async Task<IActionResult> PutFornecedor(PostFornecedorModel request) =>
            Ok(await _fornecedorService.AtualizarFornecedor(_mapper.Map<PostFornecedorCommand>(request)));

        [HttpGet]
        public async Task<IActionResult> GetFornecedores() =>
            Ok(await _fornecedorService.ConsultaFornecedores());

        [HttpDelete]
        public async Task<IActionResult> DeleteFornecedor(string cnpj) =>
            Ok(await _fornecedorService.DeletarFornecedor(cnpj));

    }
}

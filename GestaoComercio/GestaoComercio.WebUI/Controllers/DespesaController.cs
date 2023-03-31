using AutoMapper;
using GestaoComercio.Application.Models.Despesa.Commands;
using GestaoComercio.Application.Models.Fornecedor.Commands;
using GestaoComercio.Application.Models.Pedido.Commands;
using GestaoComercio.Application.Models.Precificacao.Commands;
using GestaoComercio.Application.Services;
using GestaoComercio.Domain.Entities;
using GestaoComercio.Domain.Interfaces;
using GestaoComercio.WebUI.Models;
using GestaoComercio.WebUI.Models.Despesa.Command;
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
    public class DespesaController : ControllerBase
    {
        private readonly ILogger<DespesaController> _logger;
        private readonly PedidoService _pedidoService;
        private readonly DespesaService _despesaService;
        private readonly IMapper _mapper;
        private readonly ProdutoService _produtoService;
        private readonly EspecificacoesProdutoService _especificacoesProdutoService;
        private readonly IGenericRepository<EspecificacaoProduto> _especificacaoProdutoRepository;
        private readonly IGenericRepository<Pedido> _pedidoRepository;
        private readonly IGenericRepository<Produto> _produtoRepository;
        private readonly IGenericRepository<Despesa> _despesaRepository;

        public DespesaController(ILogger<DespesaController> logger, IMapper mapper, IGenericRepository<Pedido> pedidoRepository, IGenericRepository<Produto> produtoRepository, IGenericRepository<EspecificacaoProduto> especificacaoProdutoRepository, IGenericRepository<Despesa> despesaRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _especificacoesProdutoService = new EspecificacoesProdutoService(especificacaoProdutoRepository, mapper);
            _produtoService = new ProdutoService(produtoRepository, _especificacoesProdutoService, mapper);
            _despesaService = new DespesaService(despesaRepository, mapper);
        }

        [HttpPost]
        public async Task<IActionResult> PostDespesa(PostDespesaModel request) =>
            Ok(await _despesaService.InserirDespesa(_mapper.Map<PostDespesaCommand>(request)));

        [HttpPut]
        public async Task<IActionResult> PutDespesa(PostDespesaModel request) =>
            Ok(await _despesaService.AtualizarDespesa(_mapper.Map<PostDespesaCommand>(request)));

        [HttpGet]
        public async Task<IActionResult> GetDespesas() =>
            Ok(await _despesaService.ConsultaDespesas());

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDespesa(int id) =>
            Ok(await _despesaService.DeletarDespesa(id));
    }
}

using AutoMapper;
using GestaoComercio.Application.Models.Pedido.Commands;
using GestaoComercio.Application.Models.Precificacao.Commands;
using GestaoComercio.Application.Services;
using GestaoComercio.Domain.Entities;
using GestaoComercio.Domain.Interfaces;
using GestaoComercio.WebUI.Models;
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
    public class PrecificacaoController : ControllerBase
    {
        private readonly ILogger<PrecificacaoController> _logger;
        private readonly PedidoService _pedidoService;
        private readonly IMapper _mapper;
        private readonly ProdutoService _produtoService;
        private readonly EspecificacoesProdutoService _especificacoesProdutoService;
        private readonly IGenericRepository<EspecificacaoProduto> _especificacaoProdutoRepository;
        private readonly IGenericRepository<Pedido> _pedidoRepository;
        private readonly IGenericRepository<Produto> _produtoRepository;

        public PrecificacaoController(ILogger<PrecificacaoController> logger, IMapper mapper, IGenericRepository<Pedido> pedidoRepository, IGenericRepository<Produto> produtoRepository, IGenericRepository<EspecificacaoProduto> especificacaoProdutoRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _especificacoesProdutoService = new EspecificacoesProdutoService(especificacaoProdutoRepository, mapper);
            _produtoService = new ProdutoService(produtoRepository, _especificacoesProdutoService, mapper);
        }

        [HttpPut]
        public IActionResult PostPrecificacao(PostPrecificacaoModel request)
        {
            _produtoService.UpdateValoresVenda(_mapper.Map<PostPrecificacaoCommand>(request));
            return Ok();
        }

        [HttpGet]
        public IActionResult GetPrecificacao(string codigoFornecedor) =>
            Ok(_produtoService.ConsultaPrecificacao(codigoFornecedor));
    }
}

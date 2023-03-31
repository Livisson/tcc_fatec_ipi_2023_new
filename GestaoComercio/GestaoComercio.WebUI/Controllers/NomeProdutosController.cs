using AutoMapper;
using GestaoComercio.Application.Models.Fornecedor.Commands;
using GestaoComercio.Application.Models.NomeProdutos.Commands;
using GestaoComercio.Application.Models.Pedido.Commands;
using GestaoComercio.Application.Models.Precificacao.Commands;
using GestaoComercio.Application.Services;
using GestaoComercio.Domain.Entities;
using GestaoComercio.Domain.Interfaces;
using GestaoComercio.WebUI.Models;
using GestaoComercio.WebUI.Models.Fornecedor.Commands;
using GestaoComercio.WebUI.Models.NomeProdutos.Commands;
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
    public class NomeProdutosController : ControllerBase
    {
        private readonly ILogger<NomeProdutosController> _logger;
        private readonly NomeProdutosService _nomeProdutosService;
        private readonly IGenericRepository<NomeProdutos> _nomeProdutosRepository;
        private readonly IMapper _mapper;


        public NomeProdutosController(ILogger<NomeProdutosController> logger, IMapper mapper, IGenericRepository<NomeProdutos> nomeProdutosRepository, IGenericRepository<Produto> produtoRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _nomeProdutosService = new NomeProdutosService(nomeProdutosRepository, mapper, produtoRepository);
        }

        [HttpPost]
        public async Task<IActionResult> PostNomeProdutos(PostNomeProdutosModel request) =>
            Ok(await _nomeProdutosService.InserirNomeProduto(_mapper.Map<PostNomeProdutosCommand>(request)));

        //[HttpPut]
        //public async Task<IActionResult> PutNomeProduto(PostNomeProdutosModel request) =>
        //    Ok(await _nomeProdutosService.AtualizarNomeProduto(_mapper.Map<PostNomeProdutosCommand>(request)));

        [HttpPut]
        public async Task<IActionResult> PutNomeProduto(PostNomeProdutosModel request) =>
            Ok(await _nomeProdutosService.AtualizarNomeProduto(_mapper.Map<PostNomeProdutosCommand>(request)));

        [HttpGet]
        public async Task<IActionResult> GetNomeProdutos() =>
            Ok(await _nomeProdutosService.ConsultaNomeProdutos());

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNomeProduto(int id) =>
            Ok(await _nomeProdutosService.DeletarNomeProduto(id));

    }
}

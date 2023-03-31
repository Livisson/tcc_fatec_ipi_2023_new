using GestaoComercio.Domain.Entities;
using GestaoComercio.Domain.Interfaces;
using GestaoComercio.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoComercio.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericRepository<Fornecedor> _fornecedorRepository;
        private readonly IGenericRepository<Pedido> _pedidoRepository;
        public HomeController(ILogger<HomeController> logger, IGenericRepository<Fornecedor> fornecedorRepository, IGenericRepository<Pedido> pedidoRepository)
        {
            _logger = logger;
            _fornecedorRepository = fornecedorRepository;
            _pedidoRepository = pedidoRepository;
        }

        [HttpGet("api/getPedido")]
        public async Task<IActionResult> GetPedido()
        {
            //Fornecedor forne = _fornecedorRepository.GetAsync().Result.ToList().First();
            //forne.ProdutosDoFornecedor = new List<Produto>();
            //await _fornecedorRepository.UpdateAsync(forne);

            //await _pedidoRepository.CreateAsync(new Pedido
            //{
            //    DataCompra = new DateTime(),
            //    DataVencimento = new DateTime(),
            //    Quantidade = 20,
            //    ValorCompra = 50.70,
            //    Produto = new Produto
            //    {
            //        CodigoBarras = "44444443-9",
            //        Nome = "Leite",
            //        PerDesconto = 0,
            //        PerMargem = 10,
            //        QtdEstoqueTotal = 20,
            //        ValorSugerido = 50.70 * 0.1,
            //        ValorVenda = 25.32,
            //        Fornecedor = new Fornecedor
            //        {
            //            Cnpj = "50000333/0001",
            //            Nome = "Sbuild"
            //        }
            //    }
            //});
            


            return Ok();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

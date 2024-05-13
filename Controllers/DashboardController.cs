using ControladorOnion.Context;
using ControladorOnion.Repository;
using ControladorOnion.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ControladorOnion.Controllers
{
    public class DashboardController : Controller
    {
        private readonly PedidoRepository _pedidoRepository;
        public DashboardController(ControladorContext context)
        {
            _pedidoRepository = new PedidoRepository(context);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var pedidos = await _pedidoRepository.GetAllPedidosAsync();


            if(pedidos.Count == 0)
            {
                TempData["DashboardMessageNoData"] = "Sem dados para mostrar, por favor importe alguns dados na aba 'Cadastrar Dados'";
                return View(pedidos);
            }


            // Dados para o gráfico de vendas por região
            var vendasPorRegiao = pedidos
           .GroupBy(p => p.Regiao)
           .Select(g => new { Regiao = g.Key, TotalVendas = g.Count() })
           .OrderByDescending(g => g.TotalVendas)
           .Take(5)
           .ToList();

            ViewData["ValorNorte"] = 0;
            ViewData["ValorNordeste"] = 0;
            ViewData["ValorCentroOeste"] = 0;
            ViewData["ValorSudeste"] = 0;
            ViewData["ValorSul"] = 0;
            foreach (var regiao in vendasPorRegiao)
            {
                switch (regiao.Regiao.ToLower())
                {
                    case "norte":
                        ViewData["ValorNorte"] = regiao.TotalVendas;
                        break;
                    case "nordeste":
                        ViewData["ValorNordeste"] = regiao.TotalVendas;
                        break;
                    case "centro-oeste":
                        ViewData["ValorCentroOeste"] = regiao.TotalVendas;
                        break;
                    case "sudeste":
                        ViewData["ValorSudeste"] = regiao.TotalVendas;
                        break;
                    case "sul":
                        ViewData["ValorSul"] = regiao.TotalVendas;
                        break;
                    default:
                        // Outras regiões, se houver, podem ser tratadas aqui
                        break;
                }
            }


            //Dados para o gráfico de produtos mais vendidos
            var produtosMaisVendidos = pedidos
            .GroupBy(p => p.Produto)
            .Select(g => new { Produto = g.Key, QuantidadeVendida = g.Count() })
            .OrderByDescending(g => g.QuantidadeVendida)
            .Take(5)
            .ToList();

            ViewData["ProdutosMaisVendidos"] = produtosMaisVendidos;

            return View(pedidos);
        }
    }
}

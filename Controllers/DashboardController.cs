using ControladorOnion.Context;
using ControladorOnion.Repository;
using Microsoft.AspNetCore.Mvc;

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
            return View(pedidos);
        }
    }
}

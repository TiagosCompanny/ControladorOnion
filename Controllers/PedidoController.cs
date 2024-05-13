using ControladorOnion.Context;
using ControladorOnion.Models;
using ControladorOnion.Repository;
using ControladorOnion.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ControladorOnion.Controllers
{
    public class PedidoController : Controller
    {
        private readonly PedidoRepository _pedidoRepository;
        private readonly  ProdutoRepository _produtoRepository;

        public PedidoController(ControladorContext context)
        {
            _pedidoRepository = new PedidoRepository(context);
            _produtoRepository = new ProdutoRepository(context);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var pedidos = await _pedidoRepository.GetAllPedidosAsync();
            if (pedidos.Count == 0)
            {
                TempData["PedidoMessageNoData"] = "Sem dados para mostrar, por favor importe alguns dados na aba 'Cadastrar Dados'";
                return View(pedidos);
            }

            return View(pedidos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CPFouCNPJ,NomeOuRazaoSocial,CEP,Produto,NumeroPedido,Data")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                // Verificar se o produto existe
                var produtoExistente = await _produtoRepository.GetProdutoByNameAsync(pedido.Produto);

                if (produtoExistente is not null)
                {
                    var validacaoPedido = Util.ValidarPedido(pedido);
                    if (validacaoPedido.Valido)
                    {
                        await _pedidoRepository.AddPedidoAsync(pedido);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(validacaoPedido.Campo, validacaoPedido.Erro);
                        return View(pedido);
                    }          
                }
                else
                {
                    // O produto não existe, exibir uma mensagem de erro
                    ModelState.AddModelError("Produto", "O produto selecionado não existe.");
                    return View(pedido);
                }
            }
            return View(pedido);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _pedidoRepository.GetPedidoByIdAsync(id.Value);
            if (pedido == null)
            {
                return NotFound();
            }
            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PedidoId,CPFouCNPJ,NomeOuRazaoSocial,CEP,Produto,NumeroPedido,Data")] Pedido pedido)
        {
            if (id != pedido.PedidoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var produtoExistente = await _produtoRepository.GetProdutoByNameAsync(pedido.Produto);

                if (produtoExistente is not null)
                {
                    var validacaoPedido = Util.ValidarPedido(pedido);
                    if (validacaoPedido.Valido)
                    {
                        try
                        {
                            await _pedidoRepository.UpdatePedidoAsync(pedido);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!_pedidoRepository.PedidoExists(pedido.PedidoId))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(validacaoPedido.Campo, validacaoPedido.Erro);
                        return View(pedido);
                    }

                }
                else
                {

                    ModelState.AddModelError("Produto", "O produto selecionado não existe.");
                    return View(pedido);
                }
            }
            return View(pedido);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _pedidoRepository.GetPedidoByIdAsync(id.Value);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _pedidoRepository.DeletePedidoAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

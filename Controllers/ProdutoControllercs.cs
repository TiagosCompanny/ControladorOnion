using ControladorOnion.Context;
using ControladorOnion.Models;
using ControladorOnion.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ControladorOnion.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ProdutoRepository _produtoRepository;

        public ProdutoController(ControladorContext context)
        {
            _produtoRepository = new ProdutoRepository(context);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoRepository.GetAllProdutosAsync();
            return View(produtos);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProdutoId,Nome,Valor")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                await _produtoRepository.AddProdutoAsync(produto);
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _produtoRepository.GetProdutoByIdAsync(id.Value);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProdutoId,Nome,Valor")] Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _produtoRepository.UpdateProdutoAsync(produto);
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _produtoRepository.GetProdutoByIdAsync(id.Value);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _produtoRepository.DeleteProdutoAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
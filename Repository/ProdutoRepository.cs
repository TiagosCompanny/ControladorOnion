using System.Linq;
using System.Threading.Tasks;
using ControladorOnion.Context;
using ControladorOnion.Models;
using Microsoft.EntityFrameworkCore;

namespace ControladorOnion.Repository
{
    public class ProdutoRepository
    {
        private readonly ControladorContext _context;

        public ProdutoRepository(ControladorContext context)
        {
            _context = context;
        }

        public async Task<List<Produto>> GetAllProdutosAsync()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task<Produto> GetProdutoByIdAsync(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }
        public async Task<Produto> GetProdutoByNameAsync(string nome)
        {
            return await _context.Produtos.FirstOrDefaultAsync(p => p.Nome == nome);
        }

        public async Task AddProdutoAsync(Produto produto)
        {
            _context.Add(produto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProdutoAsync(Produto produto)
        {
            _context.Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProdutoAsync(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }

        public bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.ProdutoId == id);
        }
    }
}
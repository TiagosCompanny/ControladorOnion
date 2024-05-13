using System.Linq;
using System.Threading.Tasks;
using ControladorOnion.Context;
using ControladorOnion.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
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
            var produto = await _context.Produtos.FindAsync(id);

            if (produto is not null)
                return produto;
            else
                throw new Exception("Produto não encontrado, entre em contato com a equipe de desenvolvimento");
        }
        public async Task<Produto> GetProdutoByNameAsync(string nome)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Nome == nome);

            if (produto is not null)
                return produto;
            else
                throw new Exception("Produto não encontrado, entre em contato com a equipe de desenvolvimento");
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
            Produto? produto = await _context.Produtos.FindAsync(id);

            if (produto is not null)
            {
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Erro ao deletar o produto");
            }

        }

        public bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.ProdutoId == id);
        }
    }
}
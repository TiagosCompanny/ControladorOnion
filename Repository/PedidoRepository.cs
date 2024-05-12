using ControladorOnion.Context;
using ControladorOnion.Models;
using Microsoft.EntityFrameworkCore;

namespace ControladorOnion.Repository
{
    public class PedidoRepository
    {

        private readonly ControladorContext _context;

        public PedidoRepository(ControladorContext context)
        {
            _context = context;
        }

        public async Task<List<Pedido>> GetAllPedidosAsync()
        {
            return await _context.Pedidos.ToListAsync();
        }

        public async Task CadastrarPedidosAsync(List<Pedido> pedidos)
        {

            foreach (var pedido in pedidos)
            {
                _context.Add(pedido);
            }

            await _context.SaveChangesAsync();

        }
        public async Task<Pedido> GetPedidoByIdAsync(int id)
        {
            return await _context.Pedidos.FindAsync(id);
        }

        public async Task AddPedidoAsync(Pedido pedido)
        {
            _context.Add(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePedidoAsync(Pedido pedido)
        {
            _context.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePedidoAsync(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
        }

        public bool PedidoExists(int numeroPedido)
        {
            return _context.Pedidos.Any(e => e.NumeroPedido == numeroPedido);
        }
    }
}

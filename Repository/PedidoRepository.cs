using ControladorOnion.Context;
using ControladorOnion.Models;
using ControladorOnion.Services;
using Microsoft.EntityFrameworkCore;

namespace ControladorOnion.Repository
{
    public class PedidoRepository
    {

        private readonly ControladorContext _context;
        public readonly CepApiService _ApiCep;
        public readonly ProdutoRepository _produtoRepository;

        public PedidoRepository(ControladorContext context)
        {
            _context = context;
            _ApiCep = new CepApiService();
            _produtoRepository = new ProdutoRepository(context);
        }

        public async Task<List<Pedido>> GetAllPedidosAsync()
        {
            return await _context.Pedidos.ToListAsync();
        }

        public async Task CadastrarPedidosAsync(List<Pedido> pedidos)
        {

            foreach (var pedido in pedidos)
            {
                var consultaDadosApiCep = _ApiCep.ObterUfECidade(pedido.CEP);
                pedido.TempoEntregaEmDias = _ApiCep.CalcularTempoDeEntregaEmDias(consultaDadosApiCep.Result.Uf);
                pedido.Regiao = _ApiCep.ObterRegiaoPorUF(consultaDadosApiCep.Result.Uf);
                var produtoDoPedido = await _produtoRepository.GetProdutoByNameAsync(pedido.Produto);
                pedido.ValorFrete = produtoDoPedido.Valor * _ApiCep.ObterPorcentagemCalculoFrete(consultaDadosApiCep.Result.Uf);

                _context.Add(pedido);
            }

            await _context.SaveChangesAsync();

        }
        public async Task<Pedido> GetPedidoByIdAsync(int id)
        {

            var retorno = await _context.Pedidos.FindAsync(id);
            if (retorno is not null)
                return retorno;
            else
                throw new Exception("Pedido não encontrado, entre em contato com a equipe de desenvolvimento");
        }

        public async Task AddPedidoAsync(Pedido pedido)
        {
            var consultaDadosApiCep = _ApiCep.ObterUfECidade(pedido.CEP);
            pedido.TempoEntregaEmDias = _ApiCep.CalcularTempoDeEntregaEmDias(consultaDadosApiCep.Result.Uf);
            pedido.Regiao = _ApiCep.ObterRegiaoPorUF(consultaDadosApiCep.Result.Uf);
            var produtoDoPedido = await _produtoRepository.GetProdutoByNameAsync(pedido.Produto);
            pedido.ValorFrete = produtoDoPedido.Valor * _ApiCep.ObterPorcentagemCalculoFrete(consultaDadosApiCep.Result.Uf);

            _context.Add(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePedidoAsync(Pedido pedido)
        {
            var consultaDadosApiCep = _ApiCep.ObterUfECidade(pedido.CEP);
            pedido.TempoEntregaEmDias = _ApiCep.CalcularTempoDeEntregaEmDias(consultaDadosApiCep.Result.Uf);
            pedido.Regiao = _ApiCep.ObterRegiaoPorUF(consultaDadosApiCep.Result.Uf);
            var produtoDoPedido = await _produtoRepository.GetProdutoByNameAsync(pedido.Produto);
            pedido.ValorFrete = produtoDoPedido.Valor * _ApiCep.ObterPorcentagemCalculoFrete(consultaDadosApiCep.Result.Uf);

            _context.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePedidoAsync(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if(pedido is not null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Erro ao deletar o pedido");
            }        
        }

        public bool PedidoExists(int numeroPedido)
        {
            return _context.Pedidos.Any(e => e.NumeroPedido == numeroPedido);
        }
    }
}

using ControladorOnion.Models;
using Microsoft.EntityFrameworkCore;

namespace ControladorOnion.Context
{
    public class ControladorContext : DbContext
    {

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        public ControladorContext(DbContextOptions<ControladorContext> options) : base(options)
        {
            Initialize();
        }

        public void Initialize()
        {
            if (!Produtos.Any())
            {
                Produtos.AddRange(new List<Produto>
                {
                    new Produto { Nome = "Celular", Valor = 1000 },
                    new Produto { Nome = "Notebook", Valor = 3000 },
                    new Produto { Nome = "Televisão", Valor = 5000 }
                });

                SaveChanges();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("InMemoryDatabase");
        }
    }
}
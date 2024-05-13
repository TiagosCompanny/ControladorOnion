using System.ComponentModel.DataAnnotations;

namespace ControladorOnion.Models
{
    public class Pedido
    {
        [Key]
        public int PedidoId { get; set; }
        public required string CPFouCNPJ { get; set; }
        public required string NomeOuRazaoSocial { get; set; }
        public required string CEP { get; set; }
        public required string Produto { get; set; }
        public int NumeroPedido { get; set; }

        public decimal ValorFrete { get; set; }

        public string? TempoEntregaEmDias { get; set; }
        public string? Regiao { get; set; }
        public DateTime Data { get; set; }

    }
}

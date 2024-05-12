using System.ComponentModel.DataAnnotations;

namespace ControladorOnion.Models
{
    public class Pedido
    {
        [Key]
        public int PedidoId{ get; set; }
        public string CPFouCNPJ { get; set; }
        public string NomeOuRazaoSocial { get; set; }
        public string CEP { get; set; }
        public string Produto { get; set; }
        public int NumeroPedido { get; set; }
        public DateTime Data { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace ControladorOnion.Models
{

    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do produto deve ter no máximo {1} caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O valor do produto é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do produto deve ser maior que zero.")]
        public decimal Valor { get; set; }

    }
}


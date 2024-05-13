using System.ComponentModel.DataAnnotations;

namespace ControladorOnion.Models
{
    public class Cliente
    {
        [Key]
        public required string CPFouCNPJ { get; set; }
        public required string NomeOuRazaoSocial { get; set; }
        public required string CEP { get; set; }
    }
}

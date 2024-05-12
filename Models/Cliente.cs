using System.ComponentModel.DataAnnotations;

namespace ControladorOnion.Models
{
    public class Cliente
    {
        [Key]
        public string CPFouCNPJ { get; set; }
        public string NomeOuRazaoSocial { get; set; }
        public string CEP { get; set; }
    }
}

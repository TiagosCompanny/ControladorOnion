using System.ComponentModel.DataAnnotations;

namespace ControladorOnion.Models.SystemModels
{

    public class FileModel
    {
        [Required(ErrorMessage = "Por favor, selecione um arquivo.")]
        public IFormFile InputFile { get; set; }
    }

}

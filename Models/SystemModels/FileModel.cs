﻿using System.ComponentModel.DataAnnotations;

namespace ControladorOnion.Models.SystemModels
{

    public class FileModel
    {
        [Required(ErrorMessage = "Por favor, selecione um arquivo.")]
        public required IFormFile InputFile { get; set; }
    }

}

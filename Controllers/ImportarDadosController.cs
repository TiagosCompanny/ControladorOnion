using ControladorOnion.Context;
using ControladorOnion.Models;
using ControladorOnion.Models.SystemModels;
using ControladorOnion.Repository;
using ControladorOnion.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ControladorOnion.Controllers
{
    public class ImportarDadosController : Controller
    {

        private readonly PedidoRepository _pedidoRepository;

        public ImportarDadosController(ControladorContext context)
        {
            _pedidoRepository = new PedidoRepository(context);
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadPlanilhaPedidos(FileModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.InputFile != null && model.InputFile.Length > 0)
                {

                    var filePath = Path.ChangeExtension(Path.GetTempFileName() + DateTime.Now.ToString("dd-MM-yyyy"), ".xlsx");

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model.InputFile.CopyTo(stream);
                    }

                    var (pedidos, erros) = Util.ValidarPlanilhaDePedidos(filePath);

                    System.IO.File.Delete(filePath);

                    if (erros.Any())
                    {
                        foreach (var erro in erros)
                        {
                            ModelState.AddModelError("InputFile", erro);
                        }
                        return View("Index", model);
                    }
                    if(pedidos.Count == 0)
                    {
                        ModelState.AddModelError("InputFile", "Erro ao importar dados verifique se a planilha está no formato correto");
                        return View("Index", model);
                    }
                    await _pedidoRepository.CadastrarPedidosAsync(pedidos);

                    TempData["SuccessMessage"] = "Os dados foram importados com sucesso.";

                    return RedirectToAction("Index", "ImportarDados");
                }
                else
                {
                    ModelState.AddModelError("InputFile", "Por favor, selecione um arquivo.");
                }
            }
            ModelState.AddModelError("InputFile", "Por favor, selecione um arquivo.");
            return View("Index", model);
        }

    }


}

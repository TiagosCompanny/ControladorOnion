using ControladorOnion.Context;
using ControladorOnion.Models;
using ControladorOnion.Models.SystemModels;
using ControladorOnion.Repository;
using ControladorOnion.Services;
using Microsoft.AspNetCore.Mvc;

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

                    // Validar o arquivo
                    var (pedidos, erros) = Util.ValidarPlanilhaDePedidos(filePath);

                    // Deletar o arquivo temporário
                    System.IO.File.Delete(filePath);

                    if (erros.Any())
                    {
                        // Se houver erros, adicionar mensagens de erro ao ModelState
                        foreach (var erro in erros)
                        {
                            ModelState.AddModelError("", erro);
                        }

                        // Retornar à mesma view com os erros
                        return View("Index", model);
                    }

                    await _pedidoRepository.CadastrarPedidosAsync(pedidos);


                    //TODO: Escrever uma mensagem positiva de dados importados com sucesso
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

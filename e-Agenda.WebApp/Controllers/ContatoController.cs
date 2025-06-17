using Microsoft.AspNetCore.Mvc;

namespace e_Agenda.WebApp.Controllers;

[Route("contatos")]
public class ContatoController : Controller
{
    [HttpGet]
    public IActionResult Index() {
        return View();
    }
}

using Microsoft.AspNetCore.Mvc;

namespace e_Agenda.WebApp.Controllers;

[Route("compromissos")]
public class CompromissoController : Controller
{
    [HttpGet]
    public IActionResult Index() {
        return View();
    }
}

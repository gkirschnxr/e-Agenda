using Microsoft.AspNetCore.Mvc;

namespace e_Agenda.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public HomeController(ILogger<HomeController> logger) {
        _logger = logger;
    }


    public IActionResult Index() {
        return View();
    }

    [HttpGet("erro")]
    public IActionResult Erro() {
        return View();
    }

}

using Microsoft.AspNetCore.Mvc;

namespace e_Agenda.WebApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() {
        return View();
    }
}

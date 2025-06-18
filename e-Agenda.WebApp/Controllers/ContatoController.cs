using e_Agenda.Dominio.ModuloContato;
using e_Agenda.Infraestrutura.Arquivos.Compartilhado;
using e_Agenda.Infraestrutura.Arquivos.ModuloContato;
using e_Agenda.WebApp.Extensions;
using e_Agenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_Agenda.WebApp.Controllers;

[Route("contatos")]
public class ContatoController : Controller
{
    private readonly ContextoDados contexto;
    private readonly IRepositorioContato repositorioContato;

    public ContatoController() {
        contexto = new ContextoDados(true);
        repositorioContato = new RepositorioContato(contexto);
    }

    [HttpGet]
    public IActionResult Index() { 
        var registros = repositorioContato.SelecionarRegistros();

        var visualizarVM = new VisualizarContatosViewModel(registros);

        return View(visualizarVM); 
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar() {
        var cadastrarVM = new CadastrarContatoViewModel();

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarContatoViewModel cadastrarVM) {
        var registro = cadastrarVM.ParaEntidade();

        repositorioContato.CadastrarRegistro(registro);
    
        return RedirectToAction(nameof(Index));
    }
}

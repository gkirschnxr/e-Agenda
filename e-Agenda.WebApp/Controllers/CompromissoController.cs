using e_Agenda.Dominio.ModuloCompromissos;
using e_Agenda.Infraestrutura.Arquivos.Compartilhado;
using e_Agenda.Infraestrutura.Arquivos.ModuloCompromisso;
using e_Agenda.WebApp.Extensions;
using e_Agenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_Agenda.WebApp.Controllers;

[Route("compromissos")]
public class CompromissoController : Controller
{
    private readonly ContextoDados contexto;
    private readonly IRepositorioCompromisso repositorioCompromisso;

    public CompromissoController() {
        contexto = new ContextoDados(true);
        repositorioCompromisso = new RepositorioCompromisso(contexto);
    }


    [HttpGet]
    public IActionResult Index() {
        var registros = repositorioCompromisso.SelecionarRegistros();

        var visualizarVM = new VisualizarCompromissosViewModel(registros);

        return View(visualizarVM);
    }


    [HttpGet("cadastrar")]
    public IActionResult Cadastrar() {

        var cadastrarVM = new CadastrarCompromissoViewModel();

        return View(cadastrarVM);
    }


    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarCompromissoViewModel cadastrarVM) {
        var registro = cadastrarVM.ParaEntidade();

        repositorioCompromisso.CadastrarRegistro(registro);

        return RedirectToAction(nameof(Index));
    }


    [HttpGet("editar/{id:guid}")]
    public IActionResult Editar(Guid id) {
        var registro = repositorioCompromisso.SelecionarRegistroPorId(id);

        var editarVM = new EditarCompromissoViewModel(id, registro.Assunto, registro.DataOcorrencia, registro.HoraInicio,
                                                 registro.HoraTermino, registro.Tipo);

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Guid id, EditarCompromissoViewModel editarVM) {
        var registros = repositorioCompromisso.SelecionarRegistros();

        var registroEditado = editarVM.ParaEntidade();

        repositorioCompromisso.EditarRegistro(id, registroEditado);

        return RedirectToAction(nameof(Index));
    }


    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id) {
        var registro = repositorioCompromisso.SelecionarRegistroPorId(id);

        var excluirVM = new ExcluirCompromissoViewModel(registro.Id, registro.Assunto);

        return View(excluirVM);
    }


    [HttpPost("excluir/{id:guid}")]
    public IActionResult ExcluirRegistro(Guid id) {
        repositorioCompromisso.ExcluirRegistro(id);

        return RedirectToAction(nameof(Index));
    }


    [HttpGet("detalhes/{id:guid}")]
    public IActionResult Detalhes(Guid id) {
        var registro = repositorioCompromisso.SelecionarRegistroPorId(id);

        var detalhesVM = new DetalhesCompromissoViewModel(id, registro.Assunto, registro.DataOcorrencia, registro.HoraInicio,
                                                         registro.HoraTermino, registro.Tipo);

        return View(detalhesVM);
    }
}

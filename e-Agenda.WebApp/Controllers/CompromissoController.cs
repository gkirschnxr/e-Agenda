using e_Agenda.Dominio.ModuloCompromissos;
using e_Agenda.Infraestrutura.Arquivos.Compartilhado;
using e_Agenda.WebApp.Extensions;
using e_Agenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_Agenda.WebApp.Controllers;

[Route("compromissos")]
public class CompromissoController : Controller
{
    private readonly ContextoDados _contexto;
    private readonly IRepositorioCompromisso _repositorioCompromisso;

    // inversao de controle
    public CompromissoController(ContextoDados contexto, IRepositorioCompromisso repositorioCompromisso) {
        _contexto = contexto;
        _repositorioCompromisso = repositorioCompromisso;
    }


    [HttpGet]
    public IActionResult Index() {
        var registros = _repositorioCompromisso.SelecionarRegistros();

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
        var registros = _repositorioCompromisso.SelecionarRegistros();

        foreach (var item in registros) {

            if (item.HoraInicio == cadastrarVM.HoraInicio) {
                ModelState.AddModelError("", "Já existe um compromisso nesse horário");
                return View("cadastrar");
            }

            if (item.HoraTermino == cadastrarVM.HoraTermino) {
                ModelState.AddModelError("", "Já existe um compromisso nesse horário");
                return View("cadastrar");
            } else break;
        }

        var registro = cadastrarVM.ParaEntidade();

        _repositorioCompromisso.CadastrarRegistro(registro);

        return RedirectToAction(nameof(Index));
    }


    [HttpGet("editar/{id:guid}")]
    public IActionResult Editar(Guid id) {
        var registro = _repositorioCompromisso.SelecionarRegistroPorId(id);

        var editarVM = new EditarCompromissoViewModel(id, registro.Assunto, registro.DataOcorrencia, registro.HoraInicio,
                                                 registro.HoraTermino, registro.Tipo);

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Guid id, EditarCompromissoViewModel editarVM) {
        var registros = _repositorioCompromisso.SelecionarRegistros();

        var registroEditado = editarVM.ParaEntidade();

        _repositorioCompromisso.EditarRegistro(id, registroEditado);

        return RedirectToAction(nameof(Index));
    }


    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id) {
        var registro = _repositorioCompromisso.SelecionarRegistroPorId(id);

        var excluirVM = new ExcluirCompromissoViewModel(registro.Id, registro.Assunto);

        return View(excluirVM);
    }


    [HttpPost("excluir/{id:guid}")]
    public IActionResult ExcluirRegistro(Guid id) {
        _repositorioCompromisso.ExcluirRegistro(id);

        return RedirectToAction(nameof(Index));
    }


    [HttpGet("detalhes/{id:guid}")]
    public IActionResult Detalhes(Guid id) {
        var registro = _repositorioCompromisso.SelecionarRegistroPorId(id);

        var detalhesVM = new DetalhesCompromissoViewModel(id, registro.Assunto, registro.DataOcorrencia, registro.HoraInicio,
                                                         registro.HoraTermino, registro.Tipo);

        return View(detalhesVM);
    }
}

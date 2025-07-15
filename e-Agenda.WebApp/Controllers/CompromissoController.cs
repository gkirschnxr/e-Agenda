using e_Agenda.Dominio.ModuloCompromissos;
using e_Agenda.Dominio.ModuloContato;
using eAgenda.WebApp.Extensions;
using eAgenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eAgenda.WebApp.Controllers;

[Route("compromissos")]
public class CompromissoController : Controller
{
    private readonly IRepositorioCompromisso _repositorioCompromisso;
    private readonly IRepositorioContato _repositorioContato;

    public CompromissoController(IRepositorioCompromisso repositorioCompromisso, IRepositorioContato repositorioContato) {
        _repositorioCompromisso = repositorioCompromisso;
        _repositorioContato = repositorioContato;
    }

    [HttpGet]
    public IActionResult Index() {
        var registros = _repositorioCompromisso.SelecionarRegistros();

        var visualizarVM = new VisualizarCompromissosViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar() {
        var contatosDisponiveis = _repositorioContato.SelecionarRegistros();

        var cadastrarVM = new CadastrarCompromissoViewModel(contatosDisponiveis);

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarCompromissoViewModel cadastrarVM) {
        var contatosDisponiveis = _repositorioContato.SelecionarRegistros();

        if (!ModelState.IsValid) {
            foreach (var cd in contatosDisponiveis) {
                var selecionarVM = new SelectListItem(cd.Nome, cd.Id.ToString());

                cadastrarVM.ContatosDisponiveis?.Add(selecionarVM);
            }

            return View(cadastrarVM);
        }

        var compromisso = cadastrarVM.ParaEntidade(contatosDisponiveis);

        _repositorioCompromisso.CadastrarRegistro(compromisso);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public ActionResult Editar(Guid id) {
        var contatosDisponiveis = _repositorioContato.SelecionarRegistros();

        var registroSelecionado = _repositorioCompromisso.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var editarVM = new EditarCompromissoViewModel(
            id,
            registroSelecionado.Assunto,
            registroSelecionado.Data,
            registroSelecionado.HoraInicio,
            registroSelecionado.HoraTermino,
            registroSelecionado.Tipo,
            registroSelecionado.Local,
            registroSelecionado.Link,
            registroSelecionado.Contato?.Id,
            contatosDisponiveis
        );

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(Guid id, EditarCompromissoViewModel editarVM) {
        var contatosDisponiveis = _repositorioContato.SelecionarRegistros();

        if (!ModelState.IsValid) {
            foreach (var cd in contatosDisponiveis) {
                var selecionarVM = new SelectListItem(cd.Nome, cd.Id.ToString());

                editarVM.ContatosDisponiveis?.Add(selecionarVM);
            }

            return View(editarVM);
        }

        var compromissoEditado = editarVM.ParaEntidade(contatosDisponiveis);

        _repositorioCompromisso.EditarRegistro(id, compromissoEditado);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id) {
        var registroSelecionado = _repositorioCompromisso.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var excluirVM = new ExcluirCompromissoViewModel(registroSelecionado.Id, registroSelecionado.Assunto);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult ExcluirConfirmado(Guid id) {
        var registroSelecionado = _repositorioCompromisso.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        _repositorioCompromisso.ExcluirRegistro(id);

        return RedirectToAction(nameof(Index));
    }
}
using e_Agenda.Dominio.ModuloCompromissos;
using e_Agenda.Dominio.ModuloContato;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Infraestrutura.ORM.Compartilhado;
using eAgenda.WebApp.Extensions;
using eAgenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eAgenda.WebApp.Controllers;

[Route("compromissos")]
public class CompromissoController : Controller
{
    private readonly eAgendaDbContext _dbContext;
    private readonly IRepositorioCompromisso _repositorioCompromisso;
    private readonly IRepositorioContato _repositorioContato;

    public CompromissoController(eAgendaDbContext dbContext, IRepositorioCompromisso repositorioCompromisso, IRepositorioContato repositorioContato) {
        _dbContext = dbContext; 
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

        var transacao = _dbContext.Database.BeginTransaction();

        try {
            _repositorioCompromisso.CadastrarRegistro(compromisso);

            _dbContext.SaveChanges();

            transacao.Commit();

        } catch (Exception) {
            transacao.Rollback();

            throw;
        }

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

        var transacao = _dbContext.Database.BeginTransaction();

        try {
            _repositorioCompromisso.EditarRegistro(id, compromissoEditado);

            _dbContext.SaveChanges();

            transacao.Commit();

        } catch (Exception) {
            transacao.Rollback();

            throw;
        }        

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
        var transacao = _dbContext.Database.BeginTransaction();

        try {
            _repositorioCompromisso.ExcluirRegistro(id);

            _dbContext.SaveChanges();

            transacao.Commit();

        } catch (Exception) {
            transacao.Rollback();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }
}
using e_Agenda.Dominio.ModuloTarefa;
using e_Agenda.WebApp.Extensions;
using e_Agenda.WebApp.Models;
using eAgenda.Infraestrutura.ORM.Compartilhado;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Diagnostics;

namespace e_Agenda.WebApp.Controllers;

[Route("tarefas")]
public class TarefaController : Controller
{
    private readonly eAgendaDbContext _dbContext;
    private readonly IRepositorioTarefa _repositorioTarefa;

    //inversao de controle
    public TarefaController(eAgendaDbContext dbContext, IRepositorioTarefa repositorioTarefa) {
        _dbContext = dbContext;
        _repositorioTarefa = repositorioTarefa;
    }

    [HttpGet]
    public IActionResult Index(string? status) {
        List<Tarefa> registros;

        switch (status) {
            case "pendentes": registros = _repositorioTarefa.SelecionarTarefasPendentes(); break;
            case "concluidas": registros = _repositorioTarefa.SelecionarTarefasConcluidas(); break;
            default: registros = _repositorioTarefa.SelecionarRegistros(); break;
        }
        var visualizarVM = new VisualizarTarefaViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar() {
        var cadastrarVM = new CadastrarTarefaViewModel();

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarTarefaViewModel cadastrarVM) {
        var registros = _repositorioTarefa.SelecionarRegistros();

        foreach (var item in registros) {
            if (item.Titulo.Equals(cadastrarVM.Titulo)) {
                ModelState.AddModelError("CadastroUnico", "Já existe uma tarefa registrada com este título.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(cadastrarVM);

        var entidade = cadastrarVM.ParaEntidade();

        var transacao = _dbContext.Database.BeginTransaction();

        try {
            _repositorioTarefa.CadastrarRegistro(entidade);

            _dbContext.SaveChanges();

            transacao.Commit();

        } catch (Exception) {
            transacao.Rollback();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public IActionResult Editar(Guid id) {
        var registroSelecionado = _repositorioTarefa.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var editarVM = new EditarTarefaViewModel(
                registroSelecionado.Id,
                registroSelecionado.Titulo,
                registroSelecionado.Prioridade
            );

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Guid id, EditarTarefaViewModel editarVM) {
        var registros = _repositorioTarefa.SelecionarRegistros();

        foreach (var item in registros) {
            if (!item.Id.Equals(id) && item.Titulo.Equals(editarVM.Titulo)) {
                ModelState.AddModelError("CadastroUnico", "Já existe uma tarefa registrada com este título.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(editarVM);

        var registroEditado = editarVM.ParaEntidade();

        var transacao = _dbContext.Database.BeginTransaction();

        try {
            _repositorioTarefa.EditarRegistro(id, registroEditado);

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
        var registroSelecionado = _repositorioTarefa.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var excluirVM = new ExcluirTarefaViewModel(registroSelecionado.Id, registroSelecionado.Titulo);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    public IActionResult ExcluirConfirmado(Guid id) {
        var transacao = _dbContext.Database.BeginTransaction();

        try {
            _repositorioTarefa.ExcluirRegistro(id);

            _dbContext.SaveChanges();

            transacao.Commit();

        } catch (Exception) {
            transacao.Rollback();
            
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost, Route("/tarefas/{id:guid}/alternar-status")]
    public IActionResult AlternarStatus(Guid id) {
        var tarefaSelecionada = _repositorioTarefa.SelecionarRegistroPorId(id);

        if (tarefaSelecionada is null)
            return RedirectToAction(nameof(Index));

        if (tarefaSelecionada.Concluida)
            tarefaSelecionada.MarcarPendente();
        else
            tarefaSelecionada.ConcluirTarefa();

        var transacao = _dbContext.Database.BeginTransaction();

        try {
            _repositorioTarefa.EditarRegistro(id, tarefaSelecionada);

            _dbContext.SaveChanges();

            transacao.Commit();

        } catch (Exception) {
            transacao.Rollback();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet, Route("/tarefas/{id:guid}/gerenciar-itens")]
    public IActionResult GerenciarItens(Guid id) {
        var tarefaSelecionada = _repositorioTarefa.SelecionarRegistroPorId(id);

        if (tarefaSelecionada is null)
            return RedirectToAction(nameof(Index));

        var gerenciarItensViewModel = new GerenciarItensTarefaViewModel(tarefaSelecionada);


        return View(gerenciarItensViewModel);
    }

    [HttpPost, Route("/tarefas/{id:guid}/adicionar-item")]
    public IActionResult AdicionarItem(Guid id, string tituloItem) {
        var tarefaSelecionada = _repositorioTarefa.SelecionarRegistroPorId(id);

        if (tarefaSelecionada is null)
            return RedirectToAction(nameof(Index));

        var itemSelecionado = tarefaSelecionada.AdicionarItem(tituloItem);

        var transacao = _dbContext.Database.BeginTransaction();

        try {
            _repositorioTarefa.AdicionarItem(itemSelecionado);

            _dbContext.SaveChanges();

            transacao.Commit();

        } catch (Exception) {
            transacao.Rollback();

            throw;
        }

        var gerenciarItensViewModel = new GerenciarItensTarefaViewModel(tarefaSelecionada);

        return View(nameof(GerenciarItens), gerenciarItensViewModel);
    }

    [HttpPost, Route("/tarefas/{idTarefa:guid}/alternar-status-item/{idItem:guid}")]
    public IActionResult AlternarStatusItem(Guid idTarefa, Guid idItem) {
        var tarefaSelecionada = _repositorioTarefa.SelecionarRegistroPorId(idTarefa);

        if (tarefaSelecionada is null)
            return RedirectToAction(nameof(Index));

        var itemSelecionado = tarefaSelecionada.ObterItem(idItem);

        if (itemSelecionado is null)
            return RedirectToAction(nameof(Index));

        if (!itemSelecionado.Concluido)
            tarefaSelecionada.ConcluirItem(itemSelecionado);
        else
            tarefaSelecionada.MarcarItemPendente(itemSelecionado);

        var transacao = _dbContext.Database.BeginTransaction();

        try {
            _repositorioTarefa.AtualizarItem(itemSelecionado);

            _dbContext.SaveChanges();

            transacao.Commit();

        } catch (Exception) {
            transacao.Rollback();

            throw;
        }

        var gerenciarItensViewModel = new GerenciarItensTarefaViewModel(tarefaSelecionada);

        return View(nameof(GerenciarItens), gerenciarItensViewModel);
    }


    [HttpPost, Route("/tarefas/{idTarefa:guid}/remover-item/{idItem:guid}")]
    public IActionResult RemoverItem(Guid id, Guid idItem) {
        var tarefaSelecionada = _repositorioTarefa.SelecionarRegistroPorId(id);

        if (tarefaSelecionada is null)
            return RedirectToAction(nameof(Index));

        var itemSelecionado = tarefaSelecionada.ObterItem(idItem);

        if (itemSelecionado is null)
            return RedirectToAction(nameof(Index));

        var transacao = _dbContext.Database.BeginTransaction();

        try {
            tarefaSelecionada.RemoverItem(itemSelecionado);

            _repositorioTarefa.RemoverItem(itemSelecionado);

            _dbContext.SaveChanges();

            transacao.Commit();

        } catch (Exception) {
            transacao.Rollback();

            throw;
        }

        var gerenciarItenViewModel = new GerenciarItensTarefaViewModel(tarefaSelecionada);

        return View(nameof(GerenciarItens), gerenciarItenViewModel);
    }
}

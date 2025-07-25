using e_Agenda.WebApp.ActionFilters;
using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Infraestrutura.ORM.Compartilhado;
using eAgenda.WebApp.Extensions;
using eAgenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Controllers;

[Route("categorias")]
[ValidarModelo]
public class CategoriaController : Controller
{
    private readonly eAgendaDbContext _dbContext;
    private readonly IRepositorioCategoria _repositorioCategoria;

    // inversao de controle
    public CategoriaController(eAgendaDbContext dbContext, IRepositorioCategoria repositorioCategoria) {
        _dbContext = dbContext;
        _repositorioCategoria = repositorioCategoria;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var registros = _repositorioCategoria.SelecionarRegistros();

        var visualizarVM = new VisualizarCategoriasViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        var cadastrarVM = new CadastrarCategoriaViewModel();

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarCategoriaViewModel cadastrarVM)
    {
        var registros = _repositorioCategoria.SelecionarRegistros();

        foreach (var item in registros) {

            if (item.Titulo.Equals(cadastrarVM.Titulo)) {
                ModelState.AddModelError("CadastroUnico", "Já existe uma categoria registrada com este título.");
                return View(cadastrarVM);
            }
        }

        var entidade = cadastrarVM.ParaEntidade();

        var transacao = _dbContext.Database.BeginTransaction();

        try {
            _repositorioCategoria.CadastrarRegistro(entidade);

            _dbContext.SaveChanges();

            transacao.Commit();

        } catch (Exception) {
            transacao.Rollback();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public ActionResult Editar(Guid id)
    {
        var registroSelecionado = _repositorioCategoria.SelecionarRegistroPorId(id);

        var editarVM = new EditarCategoriaViewModel(
            id,
            registroSelecionado!.Titulo
        );

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(Guid id, EditarCategoriaViewModel editarVM)
    {
        var registros = _repositorioCategoria.SelecionarRegistros();

        foreach (var item in registros)
        {
            if (!item.Id.Equals(id) && item.Titulo.Equals(editarVM.Titulo))
            {
                ModelState.AddModelError("CadastroUnico", "Já existe uma categoria registrada com este título.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(editarVM);

        var entidadeEditada = editarVM.ParaEntidade();

        var transacao = _dbContext.Database.BeginTransaction();

        try {
            _repositorioCategoria.EditarRegistro(id, entidadeEditada);

            _dbContext.SaveChanges();

            transacao.Commit();

        } catch (Exception) {
            transacao.Rollback();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        var registroSelecionado = _repositorioCategoria.SelecionarRegistroPorId(id);

        var excluirVM = new ExcluirCategoriaViewModel(registroSelecionado!.Id, registroSelecionado.Titulo);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult ExcluirConfirmado(Guid id) {
        var transacao = _dbContext.Database.BeginTransaction();

        try {
            _repositorioCategoria.ExcluirRegistro(id);

            _dbContext.SaveChanges();

            transacao.Commit();

        } catch (Exception) {
            transacao.Rollback();

            throw;
        }


        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public IActionResult Detalhes(Guid id)
    {
        var registroSelecionado = _repositorioCategoria.SelecionarRegistroPorId(id);

        var detalhesVM = new DetalhesCategoriaViewModel(
            id,
            registroSelecionado!.Titulo,
            registroSelecionado.Despesas
        );

        return View(detalhesVM);
    }
}
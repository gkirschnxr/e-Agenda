using e_Agenda.Dominio.ModuloContato;
using e_Agenda.Infraestrutura.Arquivos.Compartilhado;
using e_Agenda.Infraestrutura.Arquivos.ModuloContato;
using e_Agenda.WebApp.Extensions;
using e_Agenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_Agenda.WebApp.Controllers;

[Route("contatos")]
public class ContatoController : Controller {
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
        var registros = repositorioContato.SelecionarRegistros();

        cadastrarVM.FormatarTelefone();

        foreach (var item in registros) {
            
            if (item.Telefone == cadastrarVM.Telefone) {
                ModelState.AddModelError("CadastroUnico", "Já existe um contato com esse telefone.");
                break;
            }
        }

        foreach (var item in registros) {

            if (item.Email == cadastrarVM.Email) {
                ModelState.AddModelError("CadastroUnico", "Já existe um contato com esse e-mail.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(cadastrarVM);

        var registro = cadastrarVM.ParaEntidade();

        repositorioContato.CadastrarRegistro(registro);

        return RedirectToAction(nameof(Index));
    }


    [HttpGet("editar/{id:guid}")]
    public IActionResult Editar(Guid id) {
        var registro = repositorioContato.SelecionarRegistroPorId(id);

        var editarVM = new EditarContatoViewModel(id, registro.Nome, registro.Email, registro.Telefone);

        return View(editarVM);
    }


    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Guid id, EditarContatoViewModel editarVM) {
        var registros = repositorioContato.SelecionarRegistros();

        editarVM.FormatarTelefone();

        foreach (var item in registros) {

            if (item.Id != editarVM.Id && item.Telefone == editarVM.Telefone) {
                ModelState.AddModelError("CadastroUnico", "Já existe um contato com esse telefone.");
                break;
            }
        }

        foreach (var item in registros) {

            if (item.Id != editarVM.Id && item.Email == editarVM.Email) {
                ModelState.AddModelError("CadastroUnico", "Já existe um contato com esse e-mail.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(editarVM);

        var registroEditado = editarVM.ParaEntidade();

        repositorioContato.EditarRegistro(id, registroEditado);

        return RedirectToAction(nameof(Index));
    }


    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id) {
        var registro = repositorioContato.SelecionarRegistroPorId(id);

        var excluirVM = new ExcluirContatoViewModel(registro.Id, registro.Nome);

        return View(excluirVM);
    }


    [HttpPost("excluir/{id:guid}")]
    public IActionResult ExcluirRegistro(Guid id) {
        repositorioContato.ExcluirRegistro(id);

        return RedirectToAction(nameof(Index));
    }


    [HttpGet("detalhes/{id:guid}")]
    public IActionResult Detalhes(Guid id) {
        var registro = repositorioContato.SelecionarRegistroPorId(id);

        var detalhesVM = new DetalhesContatoViewModel(id, registro.Nome, registro.Email, registro.Telefone);

        return View(detalhesVM);
    }
}

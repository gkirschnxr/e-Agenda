using e_Agenda.Dominio.Compartilhado;
using e_Agenda.Dominio.ModuloTarefa;
using e_Agenda.Infraestrutura.Arquivos.Compartilhado;
using e_Agenda.Infraestrutura.Arquivos.ModuloTarefa;
using e_Agenda.WebApp.Extensions;
using e_Agenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_Agenda.WebApp.Controllers
{
    [Route("tarefas")]
    public class TarefaController : Controller
    {
        private readonly ContextoDados contextoDados;
        private readonly IRepositorioTarefa repositorioTarefa;

        public TarefaController()
        {
            contextoDados = new ContextoDados(true);
            repositorioTarefa = new RepositorioTarefa(contextoDados);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var registros = repositorioTarefa.SelecionarRegistros();

            var visualizarVM = new VisualizarTarefasViewModel(registros);

            return View(visualizarVM);
        }

        [HttpGet("cadastrar")]
        public IActionResult Cadastrar() 
        {
            var cadastrarVM = new CadastrarTarefaViewModel();
            
            return View(cadastrarVM);
        }

        [HttpPost("cadastrar")]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(CadastrarTarefaViewModel cadastrarVM) 
        {
            var registros = repositorioTarefa.SelecionarRegistros();

            foreach (var item in registros)
            {
                if (item.Titulo.Equals(cadastrarVM.Titulo)) 
                {
                    ModelState.AddModelError("CadastroUnico", "Já existe uma Tarefa com esse Titulo.");
                    break;
                }
            }

            var entidade = cadastrarVM.ParaEntidade();

            repositorioTarefa.CadastrarRegistro(entidade);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("editar/{id:guid}")]
        public IActionResult Editar(Guid id) 
        {
            var registroSelecionado = repositorioTarefa.SelecionarRegistroPorId(id);

            var editarVM = new EditarTarefaViewModel(
                id,
                registroSelecionado.Titulo,
                registroSelecionado.Prioridade,
                registroSelecionado.ItensTarefa
                );
            return View(editarVM);
        }

        [HttpPost("editar/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Guid id, EditarTarefaViewModel editarVM) 
        {
            var entidadeEditada = editarVM.ParaEntidade();

            repositorioTarefa.EditarRegistro(id, entidadeEditada);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("excluir/{id:guid}")]
        public IActionResult Excluir(Guid id) 
        {
            var registroSelecionado = repositorioTarefa.SelecionarRegistroPorId(id);

            var excluirVM = new ExcluirTarefaViewModel(registroSelecionado.Id, registroSelecionado.Titulo);

            return View(excluirVM);
        }

        [HttpPost("excluir/{id:guid}")]
        public IActionResult ExcluirConfirmado(Guid id) 
        {
            repositorioTarefa.ExcluirRegistro(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

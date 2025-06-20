using e_Agenda.Dominio.ModuloCompromissos;
using e_Agenda.Infraestrutura.Arquivos.Compartilhado;
using e_Agenda.Infraestrutura.Arquivos.ModuloCompromisso;
using e_Agenda.Infraestrutura.Arquivos.ModuloContato;
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

    [HttpGet("detalhes/{id:guid}")]
    public IActionResult Detalhes(Guid id) {
        var registro = repositorioCompromisso.SelecionarRegistroPorId(id);

        var detalhesVM = new DetalhesCompromissoViewModel(id, registro.Assunto, registro.DataOcorrencia, registro.HoraInicio,
                                                         registro.HoraTermino, registro.Tipo, registro.Contato);

        return View(detalhesVM);
    }
}

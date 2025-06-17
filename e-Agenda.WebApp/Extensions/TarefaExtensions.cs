using e_Agenda.Dominio.ModuloTarefa;
using e_Agenda.WebApp.Models;


namespace e_Agenda.WebApp.Extensions
{
    public static class TarefaExtensions
    {
        public static Tarefa ParaEntidade(this FormularioTarefaViewModels formularioVM) 
        {
            return new Tarefa(formularioVM.Titulo, formularioVM.Prioridade, formularioVM.ItensTarefa);
        }
        public static DetalhesTarefasViewModel ParaDetalhesVM(this Tarefa tarefa) 
        {
            return new DetalhesTarefasViewModel(
                tarefa.Id,
                tarefa.Titulo,
                tarefa.Prioridade,
                tarefa.ItensTarefa
                );
        }
    }
}

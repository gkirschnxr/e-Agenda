using e_Agenda.Dominio.ModuloTarefa;
using e_Agenda.Infraestrutura.Arquivos.Compartilhado;

namespace e_Agenda.Infraestrutura.Arquivos.ModuloTarefa
{
    public class RepositorioTarefa : RepositorioBase<Tarefa>, IRepositorioTarefa
    {
        public RepositorioTarefa(ContextoDados contexto) : base(contexto)
        {
        }

        protected override List<Tarefa> ObterRegistros()
        {
            return contexto.Tarefas;
        }
    }
}

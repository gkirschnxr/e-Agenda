using e_Agenda.Dominio.Compartilhado;

namespace e_Agenda.Dominio.ModuloTarefa
{
    public class Tarefa : EntidadeBase<Tarefa>
    {

        public string Titulo { get; set; }
        public string Prioridade { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataConclusao { get; set; }
        public bool Status = false;
        public double Percentual { get; set; }
        public string ItensTarefa { get; set; }

        public Tarefa(){}

        public Tarefa(
            string titulo, 
            string prioridade,
            string itensTarefa
            ) : this()
        {
            Id = Guid.NewGuid();
            Titulo = titulo;
            Prioridade = prioridade;
            ItensTarefa = itensTarefa;
        }

        public override void AtualizarRegistro(Tarefa registroEditado)
        {
            Titulo = registroEditado.Titulo;
            Prioridade = registroEditado.Prioridade;
            ItensTarefa = registroEditado.ItensTarefa;
        }
    }
}

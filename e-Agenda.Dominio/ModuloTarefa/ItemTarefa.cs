namespace e_Agenda.Dominio.ModuloTarefa
{
    public class ItemTarefa
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public bool Concluido { get; set; }

        // tornar FK explicita
        public Guid TarefaId { get; set; }
        public Tarefa? Tarefa { get; set; }


        public ItemTarefa() { }

        public ItemTarefa(string titulo, Tarefa tarefa) : this() {
            Id = Guid.NewGuid();
            Titulo = titulo;
            Concluido = false;
            Tarefa = tarefa;
            TarefaId = tarefa.Id;
        }

        public void MarcarPendente()
        {
            Concluido = false;
        }

        public void Concluir()
        {
            Concluido = true;           
        }

    }
}

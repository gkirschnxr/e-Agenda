namespace e_Agenda.Dominio.ModuloTarefa
{
    public class ItemTarefa
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Concluido { get; set; }
        public Tarefa? Tarefa { get; set; }


        public ItemTarefa() { }

        public ItemTarefa(string nome, Tarefa tarefa) : this() {
            Id = Guid.NewGuid();
            Nome = nome;
            Concluido = false;
            Tarefa = tarefa;
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

namespace e_Agenda.Dominio.ModuloTarefa
{
    public class ItemTarefa
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public bool Concluido { get; set; }


        public ItemTarefa() { }

        public ItemTarefa(string nome) : this()
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Concluido = false;
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

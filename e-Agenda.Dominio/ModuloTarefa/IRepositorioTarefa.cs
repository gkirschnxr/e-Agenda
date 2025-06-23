namespace e_Agenda.Dominio.ModuloTarefa
{
    public interface IRepositorioTarefa 
    {
        public void Cadastrar(Tarefa tarefa);
        public bool Editar(Guid idRegistro, Tarefa registroEditado);
        public bool Excluir(Guid idRegistro);
        Tarefa SelecionarTarefaPorId(Guid idRegistro);
        List<Tarefa> SelecionarTarefas();
        List<Tarefa> SelecionarTarefasPendentes();
        List<Tarefa> SelecionarTarefasConcluidas();
        

    }
}

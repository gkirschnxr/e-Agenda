namespace e_Agenda.Dominio.ModuloTarefa
{
    public interface IRepositorioTarefa 
    {
        public void CadastrarRegistro(Tarefa tarefa);
        public bool EditarRegistro(Guid idRegistro, Tarefa registroEditado);
        public bool ExcluirRegistro(Guid idRegistro);
        public void AdicionarItem (ItemTarefa itemTarefa);
        public bool AtualizarItem (ItemTarefa itemAtualizado);
        public bool RemoverItem (ItemTarefa itemRemovido);
        Tarefa? SelecionarRegistroPorId(Guid idRegistro);
        List<Tarefa> SelecionarRegistros();
        List<Tarefa> SelecionarTarefasPendentes();
        List<Tarefa> SelecionarTarefasConcluidas();
        

    }
}

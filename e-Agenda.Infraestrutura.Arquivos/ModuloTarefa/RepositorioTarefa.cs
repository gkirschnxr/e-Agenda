using e_Agenda.Dominio.ModuloTarefa;
using e_Agenda.Infraestrutura.Arquivos.Compartilhado;

namespace e_Agenda.Infraestrutura.Arquivos.ModuloTarefa;

public class RepositorioTarefa : IRepositorioTarefa
{
    private readonly ContextoDados _contexto;
    private readonly List<Tarefa> registros;

    public RepositorioTarefa(ContextoDados contexto) {
        _contexto = contexto;
        registros = contexto.Tarefas;
    }

    public void Cadastrar(Tarefa novaTarefa) {
        registros.Add(novaTarefa);

        _contexto.Salvar();
    }
    public bool Editar(Guid idTarefa, Tarefa tarefaEditada) {
        var tarefaSelecionada = SelecionarTarefaPorId(idTarefa);

        if (tarefaSelecionada is null)
            return false;

        tarefaSelecionada.AtualizarRegistro(tarefaEditada);

        return true;
    }

    public bool Excluir(Guid idTarefa) {
        var registroSelecionado = SelecionarTarefaPorId(idTarefa);

        if (registroSelecionado is null)
            return false;

        registros.Remove(registroSelecionado);

        _contexto.Salvar();

        return true;
    }

    public Tarefa? SelecionarTarefaPorId(Guid idTarefa) {
        return registros.Find(t => t.Id.Equals(idTarefa));
    }

    public List<Tarefa> SelecionarTarefas() {
        return registros;
    }

    public List<Tarefa> SelecionarTarefasPendentes() {
        return registros.FindAll(x => !x.Concluida);
    }

    public List<Tarefa> SelecionarTarefasConcluidas() {
        return registros.FindAll(x => x.Concluida);
    }
}

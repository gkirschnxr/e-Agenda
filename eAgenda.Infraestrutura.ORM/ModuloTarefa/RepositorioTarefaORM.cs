using e_Agenda.Dominio.ModuloTarefa;
using eAgenda.Infraestrutura.ORM.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.ORM.ModuloTarefa;
public class RepositorioTarefaORM : RepositorioBaseORM<Tarefa>, IRepositorioTarefa
{
    DbSet<Tarefa> tarefas;
    DbSet<ItemTarefa> itemTarefa;
    public RepositorioTarefaORM(eAgendaDbContext contexto) : base(contexto) {
        tarefas = contexto.Tarefas;
        itemTarefa = contexto.ItemTarefas;
    }

    public void AdicionarItem(ItemTarefa itemTarefa) {
        this.itemTarefa.Add(itemTarefa);
    }

    public bool AtualizarItem(ItemTarefa itemAtualizado) {
        var itemExistente = itemTarefa.FirstOrDefault(i => i.Id == itemAtualizado.Id);

        if (itemExistente is null) return false;

        itemExistente.Titulo = itemAtualizado.Titulo;
        itemExistente.Concluido = itemAtualizado.Concluido;

        itemTarefa.Update(itemExistente);

        return true;
    }

    public bool RemoverItem(ItemTarefa itemRemovido) {
        var itemExistente = itemTarefa.FirstOrDefault(i => i.Id == itemRemovido.Id);

        if (itemExistente is null) return false;

        this.itemTarefa.Remove(itemRemovido);
        
        return true;
    }

    public List<Tarefa> SelecionarTarefasConcluidas() {
        return tarefas
            .Include(t => t.Itens)
            .Where(t => t.Concluida)
            .ToList();
    }

    public List<Tarefa> SelecionarTarefasPendentes() {
        return tarefas
            .Include(t => t.Itens)
            .Where(t => !t.Concluida)
            .ToList();
    }

}

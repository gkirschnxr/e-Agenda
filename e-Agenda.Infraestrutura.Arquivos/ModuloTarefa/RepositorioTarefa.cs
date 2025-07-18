﻿using e_Agenda.Dominio.ModuloTarefa;
using e_Agenda.Infraestrutura.Arquivos.Compartilhado;
using eAgenda.Infraestrura.Compartilhado;
namespace e_Agenda.Infraestrutura.Arquivos.ModuloTarefa;
public class RepositorioTarefa : IRepositorioTarefa
{
    private readonly ContextoDados _contexto;
    private readonly List<Tarefa> _registros;

    public RepositorioTarefa(ContextoDados contexto) {
        _contexto = contexto;
        _registros = contexto.Tarefas;
    }

    public void CadastrarRegistro(Tarefa tarefa) {
        _registros.Add(tarefa);

        _contexto.Salvar();
    }

    public bool EditarRegistro(Guid idTarefa, Tarefa tarefaEditada) {
        var tarefaSelecionada = SelecionarRegistroPorId(idTarefa);

        if (tarefaSelecionada is null)
            return false;

        tarefaSelecionada.AtualizarRegistro(tarefaEditada);

        _contexto.Salvar();

        return true;
    }

    public bool ExcluirRegistro(Guid idTarefa) {
        var registroSelecionado = SelecionarRegistroPorId(idTarefa);

        if (registroSelecionado is null)
            return false;

        _registros.Remove(registroSelecionado);

        _contexto.Salvar();

        return true;
    }

    public void AdicionarItem(ItemTarefa item) {
        _contexto.Salvar();
    }

    public bool AtualizarItem(ItemTarefa itemAtualizado) {
        _contexto.Salvar();

        return true;
    }

    public bool RemoverItem(ItemTarefa item) {
        _contexto.Salvar();

        return true;
    }

    public Tarefa? SelecionarRegistroPorId(Guid idTarefa) {
        return _registros.Find(t => t.Id.Equals(idTarefa));
    }

    public List<Tarefa> SelecionarRegistros() {
        return _registros;
    }

    public List<Tarefa> SelecionarTarefasPendentes() {
        return _registros.FindAll(t => !t.Concluida);
    }

    public List<Tarefa> SelecionarTarefasConcluidas() {
        return _registros.FindAll(t => t.Concluida);
    }
}

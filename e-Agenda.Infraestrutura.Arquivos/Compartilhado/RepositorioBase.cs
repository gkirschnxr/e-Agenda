using e_Agenda.Dominio.Compartilhado;
using eAgenda.Infraestrura.Compartilhado;

namespace e_Agenda.Infraestrutura.Arquivos.Compartilhado;

public abstract class RepositorioBase<T> where T : EntidadeBase<T>
{
    protected ContextoDados _contexto;
    protected List<T> registros = new List<T>();

    protected RepositorioBase(ContextoDados contexto) {
        _contexto = contexto;

        registros = ObterRegistros();
    }

    protected abstract List<T> ObterRegistros();

    public void CadastrarRegistro(T novoRegistro) {
        registros.Add(novoRegistro);

        _contexto.Salvar();
    }

    public bool EditarRegistro(Guid idRegistro, T registroEditado) {
        T? registroSelecionado = SelecionarRegistroPorId(idRegistro);

        if (registroSelecionado is null)
            return false;

        registroSelecionado.AtualizarRegistro(registroEditado);

        _contexto.Salvar();

        return true;           
    }

    public bool ExcluirRegistro(Guid idRegistro) {
        T? registroSelecionado = SelecionarRegistroPorId(idRegistro);

        if (registroSelecionado is null)
            return false;

        registros.Remove(registroSelecionado);

        _contexto.Salvar();

        return true;
    }

    public List<T> SelecionarRegistros() {
        return registros;
    }

    public T? SelecionarRegistroPorId(Guid idRegistro) {
        return registros.Find((x) => x.Id.Equals(idRegistro));
    }
}
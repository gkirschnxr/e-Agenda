using e_Agenda.Dominio.Compartilhado;
using System.Data;

namespace eAgenda.Infraestrutura.BancoDeDados.Compartilhado;

public abstract class RepositorioBaseBD<T> where T : EntidadeBase<T>
{
    protected abstract string SqlInserir { get; }
    protected abstract string SqlEditar { get; }
    protected abstract string SqlExcluir { get; }
    protected abstract string SqlSelecionarPorId { get; }
    protected abstract string SqlSelecionarTodos { get; }

    protected abstract void ConfigurarParametrosRegistro(T entidade, IDbCommand comando);
    protected abstract T ConverterParaRegistro(IDataReader leitor);


    protected IDbConnection conexaoComBanco;
    protected RepositorioBaseBD(IDbConnection conexaoComBanco) {
        this.conexaoComBanco = conexaoComBanco;
    }

    public virtual void CadastrarRegistro(T novoRegistro) {
        var comandoInsercao = conexaoComBanco.CreateCommand();
        comandoInsercao.CommandText = SqlInserir;

        ConfigurarParametrosRegistro(novoRegistro, comandoInsercao);

        conexaoComBanco.Open();

        comandoInsercao.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    public virtual bool EditarRegistro(Guid idRegistro, T registroEditado) {
        var comandoEdicao = conexaoComBanco.CreateCommand();
        comandoEdicao.CommandText = SqlEditar;

        registroEditado.Id = idRegistro;

        ConfigurarParametrosRegistro(registroEditado, comandoEdicao);

        conexaoComBanco.Open();

        var linhasAfetadas = comandoEdicao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return linhasAfetadas > 0;
    }

    public virtual bool ExcluirRegistro(Guid idRegistro) {
        var comandoExclusao = conexaoComBanco.CreateCommand();
        comandoExclusao.CommandText = SqlExcluir;

        comandoExclusao.AddParameter("ID", idRegistro);

        conexaoComBanco.Open();

        var linhasAfetadas = comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return linhasAfetadas > 0;
    }

    public virtual T? SelecionarRegistroPorId(Guid idRegistro) {
        var comandoSelecao = conexaoComBanco.CreateCommand();
        comandoSelecao.CommandText = SqlSelecionarPorId;

        comandoSelecao.AddParameter("ID", idRegistro);

        conexaoComBanco.Open();

        var leitor = comandoSelecao.ExecuteReader();

        T? registro = null;

        if (leitor.Read())
            registro = ConverterParaRegistro(leitor);

        conexaoComBanco.Close();

        return registro;
    }

    public virtual List<T> SelecionarRegistros() {
        var comandoSelecao = conexaoComBanco.CreateCommand();
        comandoSelecao.CommandText = SqlSelecionarTodos;

        conexaoComBanco.Open();

        var leitor = comandoSelecao.ExecuteReader();

        var registros = new List<T>();

        while (leitor.Read()) {
            var x = ConverterParaRegistro(leitor);

            registros.Add(x);
        }

        conexaoComBanco.Close();

        return registros;
    }
}
using e_Agenda.Dominio.ModuloTarefa;
using Microsoft.Data.SqlClient;

namespace eAgenda.Infraestrutura.BancoDeDados.ModuloTarefa;
public class RepositorioTarefaBD : IRepositorioTarefa
{
    private readonly string connectionString =
    "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=eAgendaDB;Integrated Security=True";

    public void Cadastrar(Tarefa tarefa) {
        var sqlInserir = @"INSERT INTO [TBTAREFA] ([ID],[TITULO],[DATACRIACAO],[DATACONCLUSAO],
			                                       [PRIORIDADE],[CONCLUIDA])
		                                   VALUES (@ID,@TITULO,@DATACRIACAO,@DATACONCLUSAO,
			                                       @PRIORIDADE,@CONCLUIDA);"
        ;

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

        ConfigurarParametrosTarefa(tarefa, comandoInsercao);

        conexaoComBanco.Open();

        comandoInsercao.ExecuteScalar();

        conexaoComBanco.Close();
    }

    public bool Editar(Guid idRegistro, Tarefa registroEditado) {
        var sqlEditar = @"UPDATE [TBTAREFA]	SET [TITULO] = @TITULO,
			                                    [DATACRIACAO] = @DATACRIACAO,
			                                    [DATACONCLUSAO] = @DATACONCLUSAO,
			                                    [PRIORIDADE] = @PRIORIDADE,
                                                [CONCLUIDA] = @CONCLUIDA
		                                  WHERE [ID] = @ID"
        ;

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

        registroEditado.Id = idRegistro;

        ConfigurarParametrosTarefa(registroEditado, comandoEdicao);

        conexaoComBanco.Open();

        var alteracoesRealizadas = comandoEdicao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return alteracoesRealizadas > 0;
    }

    public bool Excluir(Guid idRegistro) {
        var sqlExcluir = @"DELETE FROM [TBTAREFA]
		                         WHERE [ID] = @ID"
        ;

        ExcluirItensTarefa(idRegistro);

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

        comandoExclusao.Parameters.AddWithValue("ID", idRegistro);

        conexaoComBanco.Open();

        var numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return numeroRegistrosExcluidos > 0;
    }

    public void AdicionarItem(ItemTarefa itemTarefa) {
        var sqlAdicionarItemTarefa = @"INSERT INTO [TBITEMTAREFA] ([ID],[TITULO],[CONCLUIDO],[TAREFA_ID])
		                                                   VALUES (@ID,@TITULO,@CONCLUIDO,@TAREFA_ID);"
        ;

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoInsercao = new SqlCommand(sqlAdicionarItemTarefa, conexaoComBanco);

        ConfigurarParametrosItemTarefa(itemTarefa, comandoInsercao);

        conexaoComBanco.Open();

        comandoInsercao.ExecuteScalar();

        conexaoComBanco.Close();
    }

    public bool AtualizarItem(ItemTarefa itemAtualizado) {
        var sqlEditar = @"UPDATE [TBITEMTAREFA]	SET [TITULO] = @TITULO,
			                                        [CONCLUIDO] = @CONCLUIDO,
			                                        [TAREFA_ID] = @TAREFA_ID
		                                      WHERE [ID] = @ID"
        ;

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

        ConfigurarParametrosItemTarefa(itemAtualizado, comandoEdicao);

        conexaoComBanco.Open();

        var alteracoesRealizadas = comandoEdicao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return alteracoesRealizadas > 0;
    }

    public bool RemoverItem(ItemTarefa itemRemovido) {
        var sqlExcluir = @"DELETE FROM [TBITEMTAREFA]
		                         WHERE [ID] = @ID"
        ;

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

        comandoExclusao.Parameters.AddWithValue("ID", itemRemovido.Id);

        conexaoComBanco.Open();

        var numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return numeroRegistrosExcluidos > 0;
    }

    public Tarefa? SelecionarTarefaPorId(Guid idRegistro) {
        var sqlSelecionarPorId = @"SELECT [ID],[TITULO],[PRIORIDADE],[DATACRIACAO],
			                              [DATACONCLUSAO],[CONCLUIDA]
	                                 FROM [TBTAREFA]
	                                WHERE [ID] = @ID"
        ;

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

        conexaoComBanco.Open();

        SqlDataReader leitorTarefa = comandoSelecao.ExecuteReader();

        Tarefa? tarefa = null;

        if (leitorTarefa.Read())
            tarefa = ConverterParaTarefa(leitorTarefa);

        conexaoComBanco.Close();

        return tarefa;
    }

    public List<Tarefa> SelecionarTarefas() {
        var sqlSelecionarTodos = @"SELECT [ID],[TITULO],[PRIORIDADE],
                                          [DATACRIACAO],[DATACONCLUSAO],[CONCLUIDA]
	                                 FROM [TBTAREFA]"
        ;

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

        conexaoComBanco.Open();

        SqlDataReader leitorTarefa = comandoSelecao.ExecuteReader();

        var tarefas = new List<Tarefa>();

        while (leitorTarefa.Read()) {
            var tarefa = ConverterParaTarefa(leitorTarefa);

            tarefas.Add(tarefa);
        }

        conexaoComBanco.Close();

        return tarefas;
    }

    public List<Tarefa> SelecionarTarefasConcluidas() {
        var sqlSelecionarTarefasConcluidas = @"SELECT [ID],[TITULO],[PRIORIDADE],[DATACRIACAO],
		                                              [DATACONCLUSAO],[CONCLUIDA]
	                                             FROM [TBTAREFA]
                                                WHERE [CONCLUIDA] = 1"
        ;

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTarefasConcluidas, conexaoComBanco);

        conexaoComBanco.Open();

        SqlDataReader leitorTarefa = comandoSelecao.ExecuteReader();

        var tarefasConcluidas = new List<Tarefa>();

        while (leitorTarefa.Read()) {
            var tarefa = ConverterParaTarefa(leitorTarefa);

            tarefasConcluidas.Add(tarefa);
        }

        conexaoComBanco.Close();

        return tarefasConcluidas;
    }

    public List<Tarefa> SelecionarTarefasPendentes() {
        var sqlSelecionarTarefasPendentes = @"SELECT [ID],[TITULO],[PRIORIDADE],[DATACRIACAO],
		                                             [DATACONCLUSAO],[CONCLUIDA]
	                                            FROM [TBTAREFA]
                                               WHERE [CONCLUIDA] = 0"
        ;

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTarefasPendentes, conexaoComBanco);

        conexaoComBanco.Open();

        SqlDataReader leitorTarefa = comandoSelecao.ExecuteReader();

        var tarefasPendentes = new List<Tarefa>();

        while (leitorTarefa.Read()) {
            var tarefa = ConverterParaTarefa(leitorTarefa);

            tarefasPendentes.Add(tarefa);
        }

        conexaoComBanco.Close();

        return tarefasPendentes;
    }

    private Tarefa ConverterParaTarefa(SqlDataReader leitorTarefa) {
        DateTime? dataConclusao = null;

        if (!leitorTarefa["DATACONCLUSAO"].Equals(DBNull.Value))
            dataConclusao = Convert.ToDateTime(leitorTarefa["DATACONCLUSAO"]);

        //inicializador de objeto
        var tarefa = new Tarefa {
            Id = Guid.Parse(leitorTarefa["ID"].ToString()!),
            Titulo = Convert.ToString(leitorTarefa["TITULO"])!,
            DataCriacao = Convert.ToDateTime(leitorTarefa["DATACRIACAO"]),
            DataConclusao = dataConclusao,
            Prioridade = (PrioridadeTarefa)leitorTarefa["PRIORIDADE"],
            Concluida = Convert.ToBoolean(leitorTarefa["CONCLUIDA"])
        };

        CarregarItensTarefa(tarefa);

        return tarefa;
    }

    private ItemTarefa ConverterParaItemTarefa(SqlDataReader leitorItemTarefa, Tarefa tarefa) {
        var itemTarefa = new ItemTarefa {
            Id = Guid.Parse(leitorItemTarefa["ID"].ToString()!),
            Titulo = Convert.ToString(leitorItemTarefa["TITULO"])!,
            Concluido = Convert.ToBoolean(leitorItemTarefa["CONCLUIDO"]),
            Tarefa = tarefa
        };

        return itemTarefa;
    }

    private void CarregarItensTarefa(Tarefa tarefa) {
        var sqlSelecionarItensTarefa = @"SELECT [ID],[TITULO],[CONCLUIDO],[TAREFA_ID]
	                                       FROM [TBITEMTAREFA]
	                                      WHERE [TAREFA_ID] = @TAREFA_ID"
        ;

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarItensTarefa, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("TAREFA_ID", tarefa.Id);

        conexaoComBanco.Open();

        SqlDataReader leitorItemTarefa = comandoSelecao.ExecuteReader();

        while (leitorItemTarefa.Read()) {
            var itemTarefa = ConverterParaItemTarefa(leitorItemTarefa, tarefa);

            tarefa.AdicionarItem(itemTarefa);
        }

        conexaoComBanco.Close();
    }

    private void ConfigurarParametrosTarefa(Tarefa tarefa, SqlCommand comando) {
        comando.Parameters.AddWithValue("ID", tarefa.Id);
        comando.Parameters.AddWithValue("TITULO", tarefa.Titulo);
        comando.Parameters.AddWithValue("PRIORIDADE", tarefa.Prioridade);
        comando.Parameters.AddWithValue("DATACRIACAO", tarefa.DataCriacao);
        comando.Parameters.AddWithValue("DATACONCLUSAO", tarefa.DataConclusao ?? (object)DBNull.Value);
        comando.Parameters.AddWithValue("CONCLUIDA", tarefa.Concluida);
    }

    private void ConfigurarParametrosItemTarefa(ItemTarefa item, SqlCommand comando) {
        comando.Parameters.AddWithValue("ID", item.Id);
        comando.Parameters.AddWithValue("TITULO", item.Titulo);
        comando.Parameters.AddWithValue("CONCLUIDO", item.Concluido);
        comando.Parameters.AddWithValue("TAREFA_ID", item.Tarefa!.Id);
    }

    private void ExcluirItensTarefa(Guid idTarefa) {
        var sqlExcluirItensTarefa = @"DELETE FROM [TBITEMTAREFA]
		                                    WHERE [TAREFA_ID] = @TAREFA_ID"
        ;

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoExclusao = new SqlCommand(sqlExcluirItensTarefa, conexaoComBanco);

        comandoExclusao.Parameters.AddWithValue("TAREFA_ID", idTarefa);

        conexaoComBanco.Open();

        comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();
    }
}

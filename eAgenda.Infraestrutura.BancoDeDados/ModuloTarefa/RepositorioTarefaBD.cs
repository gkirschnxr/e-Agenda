using e_Agenda.Dominio.ModuloTarefa;
using eAgenda.Infraestrutura.BancoDeDados.Compartilhado;
using System.Data;

namespace eAgenda.Infraestrutura.BancoDeDados.ModuloTarefa;
public class RepositorioTarefaBD : RepositorioBaseBD<Tarefa>, IRepositorioTarefa
{
    public RepositorioTarefaBD(IDbConnection conexaoComBanco) : base(conexaoComBanco) { }

    protected override string SqlInserir => @"
         INSERT INTO [TBTAREFA] ([ID],[TITULO],[DATACRIACAO],[DATACONCLUSAO],
			                    [PRIORIDADE],[CONCLUIDA])
		                 VALUES (@ID,@TITULO,@DATACRIACAO,@DATACONCLUSAO,
			                    @PRIORIDADE,@CONCLUIDA);";

    protected override string SqlEditar => @"
         UPDATE [TBTAREFA]	SET [TITULO] = @TITULO,
			    [DATACRIACAO] = @DATACRIACAO,
			    [DATACONCLUSAO] = @DATACONCLUSAO,
			    [PRIORIDADE] = @PRIORIDADE,
                [CONCLUIDA] = @CONCLUIDA
		  WHERE [ID] = @ID";

    protected override string SqlExcluir => @"
        DELETE FROM [TBTAREFA]
		      WHERE [ID] = @ID";

    protected override string SqlSelecionarPorId => @"
        SELECT [ID],[TITULO],[PRIORIDADE],[DATACRIACAO],
		       [DATACONCLUSAO],[CONCLUIDA]
	      FROM [TBTAREFA]
	     WHERE [ID] = @ID";

    protected override string SqlSelecionarTodos => @"
        SELECT [ID],[TITULO],[PRIORIDADE],
               [DATACRIACAO],[DATACONCLUSAO],[CONCLUIDA]
	      FROM [TBTAREFA]";

    protected string SqlAdicionarItem => @"
        INSERT INTO [TBITEMTAREFA] ([ID],[TITULO],[CONCLUIDO],[TAREFA_ID])
		                    VALUES (@ID,@TITULO,@CONCLUIDO,@TAREFA_ID);";

    protected string SqlAtualizarItem => @"
        UPDATE [TBITEMTAREFA]	
           SET [TITULO] = @TITULO,
			   [CONCLUIDO] = @CONCLUIDO,
			   [TAREFA_ID] = @TAREFA_ID
		 WHERE [ID] = @ID";

    protected string SqlRemoverItem => @"
        DELETE FROM [TBITEMTAREFA]
		      WHERE [ID] = @ID";

    protected string SqlSelecionarTarefaConcluida => @"
        SELECT [ID],[TITULO],[PRIORIDADE],[DATACRIACAO],
		       [DATACONCLUSAO],[CONCLUIDA]
          FROM [TBTAREFA]
         WHERE [CONCLUIDA] = 1";

    protected string SqlSelecionarTarefaPendente => @"
        SELECT [ID],[TITULO],[PRIORIDADE],[DATACRIACAO],
   	           [DATACONCLUSAO],[CONCLUIDA]
	      FROM [TBTAREFA]
         WHERE [CONCLUIDA] = 0";

    protected string SqlSelecionarItemTarefa => @"
        SELECT [ID],[TITULO],[CONCLUIDO],[TAREFA_ID]
	      FROM [TBITEMTAREFA]
	     WHERE [TAREFA_ID] = @TAREFA_ID";

    protected string SqlExcluirItemTarefa => @"
        DELETE FROM [TBITEMTAREFA]
		      WHERE [TAREFA_ID] = @TAREFA_ID";

    public void AdicionarItem(ItemTarefa itemTarefa) {
        var comandoInsercao = conexaoComBanco.CreateCommand();
        comandoInsercao.CommandText = SqlAdicionarItem;

        ConfigurarParametrosItemTarefa(itemTarefa, comandoInsercao);

        conexaoComBanco.Open();

        comandoInsercao.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    public bool AtualizarItem(ItemTarefa itemAtualizado) {
        var comandoEdicao = conexaoComBanco.CreateCommand();
        comandoEdicao.CommandText = SqlAtualizarItem;

        ConfigurarParametrosItemTarefa(itemAtualizado, comandoEdicao);

        conexaoComBanco.Open();

        var alteracoesRealizadas = comandoEdicao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return alteracoesRealizadas > 0;
    }

    public bool RemoverItem(ItemTarefa itemRemovido) {
        var comandoExclusao = conexaoComBanco.CreateCommand();
        comandoExclusao.CommandText = SqlRemoverItem;

        comandoExclusao.AddParameter("ID", itemRemovido.Id);

        conexaoComBanco.Open();

        var numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return numeroRegistrosExcluidos > 0;
    }

    public override bool ExcluirRegistro(Guid idTarefa) {
        ExcluirItensTarefa(idTarefa);
        return base.ExcluirRegistro(idTarefa);
    }

    public override List<Tarefa> SelecionarRegistros() {
        var registros = base.SelecionarRegistros();

        foreach (var registro in registros)
            CarregarItensTarefa(registro);

        return registros;
    }

    public override Tarefa? SelecionarRegistroPorId(Guid idRegistro) {
        var registro = base.SelecionarRegistroPorId(idRegistro);

        if (registro is not null)
            CarregarItensTarefa(registro);

        return registro;
    }

    public List<Tarefa> SelecionarTarefasConcluidas() {
        var comandoSelecao = conexaoComBanco.CreateCommand();
        comandoSelecao.CommandText = SqlSelecionarTarefaConcluida;

        conexaoComBanco.Open();

        var leitorTarefa = comandoSelecao.ExecuteReader();

        var tarefasConcluidas = new List<Tarefa>();

        while (leitorTarefa.Read()) {
            var tarefa = ConverterParaRegistro(leitorTarefa);

            tarefasConcluidas.Add(tarefa);
        }

        conexaoComBanco.Close();

        return tarefasConcluidas;
    }

    public List<Tarefa> SelecionarTarefasPendentes() {
        var comandoSelecao = conexaoComBanco.CreateCommand();
        comandoSelecao.CommandText = SqlSelecionarTarefaPendente;

        conexaoComBanco.Open();

        var leitorTarefa = comandoSelecao.ExecuteReader();

        var tarefasPendentes = new List<Tarefa>();

        while (leitorTarefa.Read()) {
            var tarefa = ConverterParaRegistro(leitorTarefa);

            tarefasPendentes.Add(tarefa);
        }

        conexaoComBanco.Close();

        return tarefasPendentes;
    }

    protected override void ConfigurarParametrosRegistro(Tarefa tarefa, IDbCommand comando) {
        comando.AddParameter("ID", tarefa.Id);
        comando.AddParameter("TITULO", tarefa.Titulo);
        comando.AddParameter("PRIORIDADE", tarefa.Prioridade);
        comando.AddParameter("DATACRIACAO", tarefa.DataCriacao);
        comando.AddParameter("DATACONCLUSAO", tarefa.DataConclusao ?? (object)DBNull.Value);
        comando.AddParameter("CONCLUIDA", tarefa.Concluida);
    }

    protected override Tarefa ConverterParaRegistro(IDataReader leitorTarefa) {
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

        return tarefa;
    }

    private void ConfigurarParametrosItemTarefa(ItemTarefa item, IDbCommand comando) {
        comando.AddParameter("ID", item.Id);
        comando.AddParameter("TITULO", item.Titulo);
        comando.AddParameter("CONCLUIDO", item.Concluido);
        comando.AddParameter("TAREFA_ID", item.Tarefa!.Id);
    }

    private ItemTarefa ConverterParaItemTarefa(IDataReader leitorItemTarefa, Tarefa tarefa) {
        var itemTarefa = new ItemTarefa {
            Id = Guid.Parse(leitorItemTarefa["ID"].ToString()!),
            Titulo = Convert.ToString(leitorItemTarefa["TITULO"])!,
            Concluido = Convert.ToBoolean(leitorItemTarefa["CONCLUIDO"]),
            Tarefa = tarefa
        };

        return itemTarefa;
    }

    private void CarregarItensTarefa(Tarefa tarefa) {
        var comandoSelecao = conexaoComBanco.CreateCommand();
        comandoSelecao.CommandText = SqlSelecionarItemTarefa;

        comandoSelecao.AddParameter("TAREFA_ID", tarefa.Id);

        try {
            conexaoComBanco.Open();
            var leitorItemTarefa = comandoSelecao.ExecuteReader();

            while (leitorItemTarefa.Read()) {
                var itemTarefa = ConverterParaItemTarefa(leitorItemTarefa, tarefa);
                tarefa.AdicionarItem(itemTarefa);
            }
        } finally {
            conexaoComBanco.Close();
        }
    }

    private void ExcluirItensTarefa(Guid idTarefa) {
        var comandoExclusao = conexaoComBanco.CreateCommand();
        comandoExclusao.CommandText = SqlExcluirItemTarefa;

        comandoExclusao.AddParameter("TAREFA_ID", idTarefa);

        conexaoComBanco.Open();

        comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();
    }
}
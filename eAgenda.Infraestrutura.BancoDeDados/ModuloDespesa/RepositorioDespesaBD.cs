using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Infraestrutura.BancoDeDados.Compartilhado;
using System.Data;

namespace eAgenda.Infraestrutura.BancoDeDados.ModuloDespesa;

public class RepositorioDespesaBD : RepositorioBaseBD<Despesa>, IRepositorioDespesa
{
    public RepositorioDespesaBD(IDbConnection conexaoComBanco) : base(conexaoComBanco) { }
    protected override string SqlInserir => @"
        INSERT INTO [TBDESPESA] ([ID],[DESCRICAO],[VALOR],[DATAOCORRENCIA],[FORMAPAGAMENTO])
                         VALUES (@ID,@DESCRICAO,@VALOR,@DATAOCORRENCIA,@FORMAPAGAMENTO);";

    protected override string SqlEditar => @"
        UPDATE [TBDESPESA] SET [DESCRICAO] = @DESCRICAO,
                               [VALOR] = @VALOR,
                               [DATAOCORRENCIA] = @DATAOCORRENCIA,
                               [FORMAPAGAMENTO] = @FORMAPAGAMENTO
                         WHERE [ID] = @ID";

    protected override string SqlExcluir => @"
        DELETE FROM [TBDESPESA]
		      WHERE [ID] = @ID";

    protected override string SqlSelecionarPorId => @"
        SELECT [ID],[DESCRICAO],[VALOR],
               [DATAOCORRENCIA],[FORMAPAGAMENTO]
	      FROM [TBDESPESA]
         WHERE [ID] = @ID";

    protected override string SqlSelecionarTodos => @"
        SELECT [ID],[DESCRICAO],[VALOR],
               [DATAOCORRENCIA],[FORMAPAGAMENTO]
	      FROM [TBDESPESA]";

    protected string SqlAdicionarCategoria => @"
        INSERT INTO [TBDESPESA_TBCATEGORIA] ([DESPESA_ID],[CATEGORIA_ID])
                                     VALUES (@DESPESA_ID,@CATEGORIA_ID)";

    protected string SqlRemoverCategoria => @"
        DELETE FROM [TBDESPESA_TBCATEGORIA]
              WHERE [DESPESA_ID] = @DESPESA_ID";

    protected string SqlCarregarCategoria => @"
        SELECT CAT.[ID], CAT.[TITULO]
          FROM [TBCATEGORIA] AS CAT INNER JOIN [TBDESPESA_TBCATEGORIA] AS DC
            ON CAT.[ID] = DC.[CATEGORIA_ID]
         WHERE DC.[DESPESA_ID] = @DESPESA_ID";

    public override void CadastrarRegistro(Despesa novoRegistro) {
        base.CadastrarRegistro(novoRegistro);
        AdicionarCategorias(novoRegistro);
    }

    public override bool EditarRegistro(Guid idRegistro, Despesa registroEditado) {
        var resultado = base.EditarRegistro(idRegistro, registroEditado);

        RemoverCategorias(idRegistro);
        AdicionarCategorias(registroEditado);

        return resultado;
    }

    public override bool ExcluirRegistro(Guid idRegistro) {
        RemoverCategorias(idRegistro);

        return base.ExcluirRegistro(idRegistro);
    }

    public override Despesa? SelecionarRegistroPorId(Guid idRegistro) {
        var registro = base.SelecionarRegistroPorId(idRegistro);

        if (registro is not null)
            CarregarCategorias(registro);

        return registro;
    }

    public override List<Despesa> SelecionarRegistros() {
        var registros = base.SelecionarRegistros();

        foreach (var registro in registros)
            CarregarCategorias(registro);

        return registros;
    }

    protected override void ConfigurarParametrosRegistro(Despesa entidade, IDbCommand comando) {
        comando.AddParameter("ID", entidade.Id);
        comando.AddParameter("DESCRICAO", entidade.Descricao);
        comando.AddParameter("VALOR", entidade.Valor);
        comando.AddParameter("DATAOCORRENCIA", entidade.DataOcorencia);
        comando.AddParameter("FORMAPAGAMENTO", (int)entidade.FormaPagamento);
    }

    protected override Despesa ConverterParaRegistro(IDataReader leitor) {
        var registro = new Despesa {
            Id = Guid.Parse(leitor["ID"].ToString()!),
            Descricao = Convert.ToString(leitor["DESCRICAO"])!,
            Valor = Convert.ToDecimal(leitor["VALOR"])!,
            DataOcorencia = Convert.ToDateTime(leitor["DATAOCORRENCIA"])!,
            FormaPagamento = (FormaPagamento)leitor["FORMAPAGAMENTO"]!,
        };

        return registro;
    }

    private Categoria ConverterParaCategoria(IDataReader leitor) {
        var registro = new Categoria {
            Id = Guid.Parse(leitor["ID"].ToString()!),
            Titulo = Convert.ToString(leitor["TITULO"])!
        };

        return registro;
    }

    private void AdicionarCategorias(Despesa despesa) {
        conexaoComBanco.Open();

        foreach (var cat in despesa.Categorias) {
            var comandoInsercao = conexaoComBanco.CreateCommand();
            comandoInsercao.CommandText = SqlAdicionarCategoria;

            comandoInsercao.AddParameter("DESPESA_ID", despesa.Id);
            comandoInsercao.AddParameter("CATEGORIA_ID", cat.Id);

            comandoInsercao.ExecuteNonQuery();
        }

        conexaoComBanco.Close();
    }

    private void RemoverCategorias(Guid idDespesa) {
        conexaoComBanco.Open();

        var comandoExclusao = conexaoComBanco.CreateCommand();
        comandoExclusao.CommandText = SqlRemoverCategoria;

        comandoExclusao.AddParameter("DESPESA_ID", idDespesa);

        comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    private void CarregarCategorias(Despesa despesa) {
        var comandoSelecao = conexaoComBanco.CreateCommand();
        comandoSelecao.CommandText = SqlCarregarCategoria;

        conexaoComBanco.Open();

        comandoSelecao.AddParameter("DESPESA_ID", despesa.Id);

        var leitorCategoria = comandoSelecao.ExecuteReader();

        while (leitorCategoria.Read()) {
            var categoria = ConverterParaCategoria(leitorCategoria);

            despesa.RegistrarCategoria(categoria);
        }

        conexaoComBanco.Close();
    }
}
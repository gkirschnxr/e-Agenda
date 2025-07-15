using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Infraestrutura.BancoDeDados.Compartilhado;
using System.Data;

namespace eAgenda.Infraestrutura.BancoDeDados.ModuloCategoria;

public class RepositorioCategoriaBD : RepositorioBaseBD<Categoria>, IRepositorioCategoria
{
    public RepositorioCategoriaBD(IDbConnection conexaoComBanco) : base(conexaoComBanco) { }

    protected override string SqlInserir => @"
        INSERT INTO [TBCATEGORIA] ([ID],[TITULO])
                           VALUES (@ID,@TITULO);";

    protected override string SqlEditar => @"
        UPDATE [TBCATEGORIA]
           SET [TITULO] = @TITULO
         WHERE [ID] = @ID";

    protected override string SqlExcluir => @"
        DELETE FROM [TBCATEGORIA]
              WHERE [ID] = @ID";

    protected override string SqlSelecionarPorId => @"
        SELECT [ID], [TITULO]
          FROM [TBCATEGORIA]
         WHERE [ID] = @ID";

    protected override string SqlSelecionarTodos => @"
        SELECT [ID], [TITULO]
          FROM [TBCATEGORIA]";

    protected string SqlSelecionarDespesasDaCategoria => @"
        SELECT D.[ID], D.[DESCRICAO], D.[VALOR],
               D.[DATAOCORRENCIA], D.[FORMAPAGAMENTO]
          FROM [TBDESPESA] AS D INNER JOIN [TBDESPESA_TBCATEGORIA] AS DC
            ON D.[ID] = DC.[DESPESA_ID]
         WHERE DC.[CATEGORIA_ID] = @CATEGORIA_ID";

    public override Categoria? SelecionarRegistroPorId(Guid idRegistro) {
        var registro = base.SelecionarRegistroPorId(idRegistro);

        if (registro is not null) CarregarDespesas(registro);

        return registro;
    }

    // SE QUISER VISUALIZAR AS DESPESAS DIRETO NO INDEX DAS CATEGORIAS, UTILIZAR ESSE MÉTODO:
    //public override List<Categoria> SelecionarRegistros() {
    //    var registros = base.SelecionarRegistros();

    //    foreach (var r in registros) {
    //        CarregarDespesas(r);
    //    }

    //    return registros;
    //}

    private Despesa ConverterParaDespesa(IDataReader leitor) {
        var registro = new Despesa {
            Id = Guid.Parse(leitor["ID"].ToString()!),
            Descricao = Convert.ToString(leitor["DESCRICAO"])!,
            Valor = Convert.ToDecimal(leitor["VALOR"])!,
            DataOcorencia = Convert.ToDateTime(leitor["DATAOCORRENCIA"])!,
            FormaPagamento = (FormaPagamento)leitor["FORMAPAGAMENTO"]!,
        };

        return registro;
    }

    private void CarregarDespesas(Categoria categoria) {
        var comandoSelecao = conexaoComBanco.CreateCommand();
        comandoSelecao.CommandText = SqlSelecionarDespesasDaCategoria;

        conexaoComBanco.Open();

        comandoSelecao.AddParameter("CATEGORIA_ID", categoria.Id);

        var leitorCategoria = comandoSelecao.ExecuteReader();

        while (leitorCategoria.Read()) {
            var despesa = ConverterParaDespesa(leitorCategoria);

            despesa.RegistrarCategoria(categoria);
        }

        conexaoComBanco.Close();
    }

    protected override void ConfigurarParametrosRegistro(Categoria entidade, IDbCommand comando) {
        comando.AddParameter("ID", entidade.Id);
        comando.AddParameter("TITULO", entidade.Titulo);
    }

    protected override Categoria ConverterParaRegistro(IDataReader leitor) {
        var registro = new Categoria {
            Id = Guid.Parse(leitor["ID"].ToString()!),
            Titulo = Convert.ToString(leitor["TITULO"])!
        };

        return registro;
    }
}
using e_Agenda.Dominio.ModuloContato;
using eAgenda.Infraestrutura.BancoDeDados.Compartilhado;
using Microsoft.Data.SqlClient;
using System.Data;

namespace eAgenda.Infraestrutura.BancoDeDados.ModuloContato;

public class RepositorioContatoBD : RepositorioBaseBD<Contato>, IRepositorioContato
{
    public RepositorioContatoBD(IDbConnection conexaoComBanco) : base(conexaoComBanco) { }

    protected override string SqlInserir => @"
        INSERT INTO [TBCONTATO] ( [ID],[NOME],[EMAIL],[TELEFONE],[EMPRESA],[CARGO] )
                         VALUES ( @ID, @NOME, @EMAIL, @TELEFONE, @EMPRESA, @CARGO );";

    protected override string SqlEditar => @"
        UPDATE [TBCONTATO]
           SET [NOME] = @NOME,
               [EMAIL] = @EMAIL,
               [TELEFONE] = @TELEFONE,
               [EMPRESA] = @EMPRESA,
               [CARGO] = @CARGO
         WHERE [ID] = @ID";

    protected override string SqlExcluir => @"
        DELETE FROM [TBCONTATO]
              WHERE [ID] = @ID";

    protected override string SqlSelecionarPorId => @"
        SELECT [ID],[NOME],[EMAIL],[TELEFONE],[EMPRESA],[CARGO]
          FROM [TBCONTATO]
         WHERE [ID] = @ID";

    protected override string SqlSelecionarTodos => @"
        SELECT [ID],[NOME],[EMAIL],[TELEFONE],[EMPRESA],[CARGO]
          FROM [TBCONTATO]";

    protected override void ConfigurarParametrosRegistro(Contato entidade, IDbCommand comando) {
        comando.AddParameter("ID", entidade.Id);
        comando.AddParameter("NOME", entidade.Nome);
        comando.AddParameter("EMAIL", entidade.Email);
        comando.AddParameter("TELEFONE", entidade.Telefone);
        comando.AddParameter("EMPRESA", entidade.Empresa ?? (object)DBNull.Value);
        comando.AddParameter("CARGO", entidade.Cargo ?? (object)DBNull.Value);
    }

    protected override Contato ConverterParaRegistro(IDataReader leitor) {
        var contato = new Contato(
            Convert.ToString(leitor["NOME"])!,
            Convert.ToString(leitor["EMAIL"])!,
            Convert.ToString(leitor["TELEFONE"])!,
            Convert.ToString(leitor["EMPRESA"]),
            Convert.ToString(leitor["CARGO"]));

        contato.Id = Guid.Parse(leitor["ID"].ToString()!);

        return contato;
    }
}
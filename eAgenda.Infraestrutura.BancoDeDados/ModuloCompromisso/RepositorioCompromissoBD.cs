﻿using e_Agenda.Dominio.ModuloCompromissos;
using e_Agenda.Dominio.ModuloContato;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Infraestrutura.BancoDeDados.Compartilhado;
using System.Data;

namespace eAgenda.Infraestrutura.BancoDeDados.ModuloCompromisso;
public class RepositorioCompromissoBD : RepositorioBaseBD<Compromisso>, IRepositorioCompromisso
{
    public RepositorioCompromissoBD(IDbConnection conexaoComBanco) : base(conexaoComBanco) { }

    protected override string SqlInserir => @"
        INSERT INTO [TBCOMPROMISSO] ([ID],[ASSUNTO],[DATA], [HORAINICIO],[HORATERMINO],
                                    [TIPO],[LOCAL],[LINK],[CONTATO_ID])
                             VALUES (@ID, @ASSUNTO, @DATA, @HORAINICIO, @HORATERMINO,
                                    @TIPO, @LOCAL, @LINK, @CONTATO_ID);";

    protected override string SqlEditar => @"
        UPDATE [TBCOMPROMISSO] SET [ASSUNTO] = @ASSUNTO,
                                   [DATA] = @DATA, 
                                   [HORAINICIO] = @HORAINICIO, 
                                   [HORATERMINO] = @HORATERMINO,
                                   [TIPO] = @TIPO,
                                   [LOCAL] = @LOCAL, 
                                   [LINK] = @LINK,
                                   [CONTATO_ID] = @CONTATO_ID
                             WHERE [ID] = @ID";

    protected override string SqlExcluir => @"
        DELETE FROM [TBCOMPROMISSO]
		      WHERE [ID] = @ID";

    protected override string SqlSelecionarPorId => @"
        SELECT CP.[ID], CP.[ASSUNTO], CP.[DATA], CP.[HORAINICIO], CP.[HORATERMINO],
               CP.[TIPO], CP.[LOCAL], CP.[LINK], CP.[CONTATO_ID],
               CT.[NOME], CT.[EMAIL], CT.[TELEFONE], CT.[CARGO], CT.[EMPRESA]
          FROM [TBCompromisso] AS CP LEFT JOIN [TBContato] AS CT
            ON CT.ID = CP.CONTATO_ID
         WHERE CP.[ID] = @ID;";

    protected override string SqlSelecionarTodos => @"
        SELECT CP.[ID], CP.[ASSUNTO], CP.[DATA], CP.[HORAINICIO], CP.[HORATERMINO],
               CP.[TIPO], CP.[LOCAL], CP.[LINK], CP.[CONTATO_ID],
               CT.[NOME], CT.[EMAIL], CT.[TELEFONE], CT.[EMPRESA], CT.[CARGO]
          FROM [TBCompromisso] AS CP LEFT JOIN [TBContato] AS CT
            ON CT.ID = CP.CONTATO_ID";

    protected override Compromisso ConverterParaRegistro(IDataReader leitorCompromisso) {
        var horaInicio = TimeSpan.FromTicks(Convert.ToInt64(leitorCompromisso["HORAINICIO"]));
        var horaTermino = TimeSpan.FromTicks(Convert.ToInt64(leitorCompromisso["HORATERMINO"]));
        Contato? contato = null;

        if (leitorCompromisso["CONTATO_ID"] != DBNull.Value)
            contato = ConverterParaContato(leitorCompromisso);


        var compromisso = new Compromisso(
            Convert.ToString(leitorCompromisso["ASSUNTO"])!,
            Convert.ToDateTime(leitorCompromisso["DATA"]),
            horaInicio,
            horaTermino,
            (TipoCompromisso)Convert.ToInt32(leitorCompromisso["TIPO"]),
            Convert.ToString(leitorCompromisso["LOCAL"])!,
            Convert.ToString(leitorCompromisso["LINK"])!,
            contato
        );

        compromisso.Id = Guid.Parse(leitorCompromisso["ID"].ToString()!);

        return compromisso;
    }

    private Contato ConverterParaContato(IDataReader leitor) {
        var contato = new Contato(
            Convert.ToString(leitor["NOME"])!,
            Convert.ToString(leitor["EMAIL"])!,
            Convert.ToString(leitor["TELEFONE"])!,
            Convert.ToString(leitor["EMPRESA"]),
            Convert.ToString(leitor["CARGO"]));

        contato.Id = Guid.Parse(leitor["ID"].ToString()!);

        return contato;
    }

    protected override void ConfigurarParametrosRegistro(Compromisso compromisso, IDbCommand comando) {
        comando.AddParameter("ID", compromisso.Id);
        comando.AddParameter("ASSUNTO", compromisso.Assunto);
        comando.AddParameter("DATA", compromisso.Data);

        //converter para ticks para nao dar problema no Banco de Dados
        comando.AddParameter("HORAINICIO", compromisso.HoraInicio.Ticks);
        comando.AddParameter("HORATERMINO", compromisso.HoraTermino.Ticks);

        comando.AddParameter("TIPO", (int)compromisso.Tipo);

        //mostrar que o comando pode ser anulavel, se faz dessa forma para o SQL:
        comando.AddParameter("LOCAL", compromisso.Local ?? (object)DBNull.Value);
        comando.AddParameter("LINK", compromisso.Link ?? (object)DBNull.Value);
        comando.AddParameter("CONTATO_ID", compromisso.Contato?.Id ?? (object)DBNull.Value);
    }
}
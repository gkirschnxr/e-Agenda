using e_Agenda.Infraestrutura.Arquivos.Compartilhado;
using eAgenda.Dominio.ModuloDespesa;

namespace eAgenda.Infraestrutura.ModuloDespesa;

public class RepositorioDespesa : RepositorioBase<Despesa>, IRepositorioDespesa
{
    public RepositorioDespesa(ContextoDados contexto) : base(contexto) { }

    protected override List<Despesa> ObterRegistros()
    {
        return contexto.Despesas;
    }
}
using e_Agenda.Dominio.ModuloContato;
using e_Agenda.Infraestrutura.Arquivos.Compartilhado;

namespace e_Agenda.Infraestrutura.Arquivos.ModuloContato;

public class RepositorioContato : RepositorioBase<Contato>, IRepositorioContato
{
    public RepositorioContato(ContextoDados contexto) : base(contexto) { }

    protected override List<Contato> ObterRegistros() {
        return _contexto.Contatos;
    }
}

using e_Agenda.Dominio.ModuloCompromissos;
using e_Agenda.Infraestrutura.Arquivos.Compartilhado;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Infraestrura.Compartilhado;

namespace eAgenda.Infraestrutura.ModuloCompromisso;

public class RepositorioCompromisso : RepositorioBase<Compromisso>, IRepositorioCompromisso
{
    public RepositorioCompromisso(ContextoDados contexto) : base(contexto) { }

    protected override List<Compromisso> ObterRegistros() {
        return _contexto.Compromissos;
    }
}
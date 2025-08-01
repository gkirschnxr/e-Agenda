using e_Agenda.Dominio.ModuloCompromissos;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Infraestrutura.ORM.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.ORM.ModuloCompromisso;
public class RepositorioCompromissoORM : RepositorioBaseORM<Compromisso>, IRepositorioCompromisso
{
    public RepositorioCompromissoORM(eAgendaDbContext contexto) : base(contexto) { }

    public override Compromisso? SelecionarRegistroPorId(Guid id) {
        return _contexto
            .Include(c => c.Contato)
            .FirstOrDefault(x => x.Id.Equals(id));
    }

    public override List<Compromisso> SelecionarRegistros() {
        return _contexto
            .Include(c => c.Contato)
            .ToList();
    }
}

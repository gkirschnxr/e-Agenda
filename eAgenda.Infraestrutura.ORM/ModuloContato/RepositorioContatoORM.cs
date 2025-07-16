using e_Agenda.Dominio.ModuloContato;
using eAgenda.Infraestrutura.ORM.Compartilhado;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace eAgenda.Infraestrutura.ORM.ModuloContato;
public class RepositorioContatoORM : RepositorioBaseORM<Contato>, IRepositorioContato
{
    public RepositorioContatoORM(eAgendaDbContext contexto) : base(contexto) { }
}

using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Infraestrutura.ORM.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.ORM.ModuloCategoria;
public class RepositorioCategoriaORM : RepositorioBaseORM<Categoria>, IRepositorioCategoria
{
    public RepositorioCategoriaORM(eAgendaDbContext contexto) : base(contexto) { }

    public override Categoria? SelecionarRegistroPorId(Guid idRegistro) {
        return _contexto
            .Include(c => c.Despesas)
            .FirstOrDefault(x => x.Id.Equals(idRegistro));
    }

    public override List<Categoria> SelecionarRegistros() {
        return _contexto
            .Include(c => c.Despesas)
            .ToList();
    }
}

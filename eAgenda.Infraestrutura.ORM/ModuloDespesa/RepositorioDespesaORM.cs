using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Infraestrutura.ORM.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.ORM.ModuloDespesa;
public class RepositorioDespesaORM : RepositorioBaseORM<Despesa>, IRepositorioDespesa
{
    public RepositorioDespesaORM(eAgendaDbContext contexto) : base(contexto) { }

    public override Despesa? SelecionarRegistroPorId(Guid idRegistro) {
        return _contexto
            .Include(d => d.Categorias)
            .FirstOrDefault(d => d.Id.Equals(idRegistro));
    }

    public override List<Despesa> SelecionarRegistros() {
        return _contexto
            .Include(d => d.Categorias)
            .ToList();
    }
}

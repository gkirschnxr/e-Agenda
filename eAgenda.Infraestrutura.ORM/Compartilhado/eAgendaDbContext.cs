using e_Agenda.Dominio.ModuloContato;
using eAgenda.Infraestrutura.ORM.ModuloContato;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.ORM.Compartilhado;

public class eAgendaDbContext : DbContext
{
    public DbSet<Contato> Contatos { get; set; }

    public eAgendaDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfiguration(new MapeadorContato());

        base.OnModelCreating(modelBuilder);
    }
}

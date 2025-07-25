using e_Agenda.Dominio.ModuloContato;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Infraestrutura.ORM.ModuloCompromisso;
using eAgenda.Infraestrutura.ORM.ModuloContato;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.ORM.Compartilhado;

public class eAgendaDbContext : DbContext
{
    public DbSet<Contato> Contatos { get; set; }
    public DbSet<Compromisso> Compromissos { get; set; }

    public eAgendaDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        var assembly = typeof(eAgendaDbContext).Assembly;

        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(modelBuilder);
    }
}

using e_Agenda.Dominio.ModuloContato;
using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Infraestrutura.ORM.ModuloCompromisso;
using eAgenda.Infraestrutura.ORM.ModuloContato;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.ORM.Compartilhado;

public class eAgendaDbContext : DbContext
{
    public DbSet<Contato> Contatos { get; set; }
    public DbSet<Compromisso> Compromissos { get; set; }

    /* o que deveria ser implementado antes da criação das tabelas:

           public DbSet<Categoria> Categorias { get; set; }
    
           public DbSet<Despesa> Despesas { get; set; }           */

    public eAgendaDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        var assembly = typeof(eAgendaDbContext).Assembly;

        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(modelBuilder);
    }
}

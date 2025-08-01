using e_Agenda.Dominio.ModuloTarefa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.ORM.ModuloTarefa;
public class MapeadorTarefa : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder) {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Titulo)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Prioridade)
            .HasConversion<string>() //conversao para banco armazenar os nomes inves dos numeros 0, 1, 2..
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.DataCriacao)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(x => x.DataConclusao)
            .HasColumnType("datetime2")
            .IsRequired(false);

        builder.Property(x => x.Concluida)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Ignore(x => x.PercentualConcluido);

        builder.HasMany(x => x.Itens)
            .WithOne(x => x.Tarefa)
            .HasForeignKey(x => x.TarefaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

using e_Agenda.Dominio.ModuloTarefa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.ORM.ModuloTarefa;
public class MapeadorItemTarefa : IEntityTypeConfiguration<ItemTarefa>
{
    public void Configure(EntityTypeBuilder<ItemTarefa> builder) {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Titulo)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Concluido)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.TarefaId)
            .IsRequired();

        builder.HasOne(x => x.Tarefa)
               .WithMany(t => t.Itens)
               .HasForeignKey(x => x.TarefaId);
    }
}

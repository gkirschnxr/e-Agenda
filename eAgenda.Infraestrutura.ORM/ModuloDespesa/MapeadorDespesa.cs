using eAgenda.Dominio.ModuloDespesa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.ORM.ModuloDespesa;
public class MapeadorDespesa : IEntityTypeConfiguration<Despesa>
{
    public void Configure(EntityTypeBuilder<Despesa> builder) {
        builder.Property(d => d.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(d => d.Descricao)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Valor)
            .IsRequired();

        builder.Property(d => d.DataOcorencia)
            .IsRequired();

        builder.Property(d => d.FormaPagamento)
            .IsRequired();

        builder.HasMany(d => d.Categorias)
            .WithMany(c => c.Despesas);
    }
}

using eAgenda.Dominio.ModuloCategoria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.ORM.ModuloCategoria;
public class MapeadorCategoria : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder) {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Titulo)
            .IsRequired();

        builder.HasMany(c => c.Despesas)
            .WithMany(d => d.Categorias);
    }
}

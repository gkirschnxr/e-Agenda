using e_Agenda.Infraestrutura.Arquivos.Compartilhado;
using eAgenda.Dominio.ModuloCategoria;

namespace e_Agenda.Infraestrutura.Arquivos.ModuloCategoria;

public class RepositorioCategoria : RepositorioBase<Categoria>, IRepositorioCategoria
{
    public RepositorioCategoria(ContextoDados contexto) : base(contexto) { }

    protected override List<Categoria> ObterRegistros()
    {
        return contexto.Categorias;
    }
}
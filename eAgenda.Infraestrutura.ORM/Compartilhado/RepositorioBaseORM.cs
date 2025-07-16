using e_Agenda.Dominio.Compartilhado;
using e_Agenda.Dominio.ModuloContato;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.ORM.Compartilhado;
public abstract class RepositorioBaseORM<T> where T : EntidadeBase<T>
{
    private readonly DbSet<T> _contexto;

    protected RepositorioBaseORM(eAgendaDbContext contexto) {
        _contexto = contexto.Set<T>();
    }

    public void CadastrarRegistro(T novoRegistro) {
        _contexto.Add(novoRegistro);
    }

    public bool EditarRegistro(Guid idRegistro, T registroEditado) {
        var registro = SelecionarRegistroPorId(idRegistro);

        if (registro is null) return false;

        registro.AtualizarRegistro(registroEditado);

        return true;
    }

    public bool ExcluirRegistro(Guid idRegistro) {
        var registro = SelecionarRegistroPorId(idRegistro);

        if (registro is null) return false;

        _contexto.Remove(registro);        

        return true;
    }

    public T? SelecionarRegistroPorId(Guid idRegistro) {
        return _contexto.FirstOrDefault(x => x.Id.Equals(idRegistro));
    }

    public List<T> SelecionarRegistros() {
        return _contexto.ToList();
    }
}

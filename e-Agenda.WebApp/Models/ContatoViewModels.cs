using e_Agenda.Dominio.ModuloContato;
using e_Agenda.WebApp.Extensions;

namespace e_Agenda.WebApp.Models;

public abstract class FormularioContatoViewModel {
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;

    //public string Cargo { get; set; }
    //public string Empresa { get; set; }
}

public class CadastrarContatoViewModel : FormularioContatoViewModel {
    public CadastrarContatoViewModel() { }
    public CadastrarContatoViewModel(string nome, string email, string telefone) : this() {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        //Cargo = cargo;
        //Empresa = empresa;
    }
}

public class EditarContatoViewModel : FormularioContatoViewModel {
    public Guid Id { get; set; }
    public EditarContatoViewModel() { }
    public EditarContatoViewModel(Guid id, string nome, string email, string telefone) : this() {
        Id = id;
        Nome = nome;
        Email = email;
        Telefone = telefone;
        //Cargo = cargo;
        //Empresa = empresa;
    }
}

public class ExcluirContatoViewModel {
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public ExcluirContatoViewModel() { }
    public ExcluirContatoViewModel(Guid id, string nome) : this() {
        Id = id;
        Nome = nome;
    }
}

public class VisualizarContatosViewModel {
    public List<DetalhesContatoViewModel> Registros { get; set; }

    public VisualizarContatosViewModel(List<Contato> contatos) {
        Registros = [];

        foreach (var c in contatos) {
            var detalhesVM = c.ParaDetalhesVM();

            if (detalhesVM != null)
               Registros.Add(detalhesVM);
        }
    }
}

public class DetalhesContatoViewModel {
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;

    public DetalhesContatoViewModel(Guid id, string nome, string email, string telefone) {
        Id = id;
        Nome = nome;
        Email = email;
        Telefone = telefone;
    }
}

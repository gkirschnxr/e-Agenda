using e_Agenda.Dominio.ModuloContato;
using e_Agenda.WebApp.Extensions;
using System.ComponentModel.DataAnnotations;

namespace e_Agenda.WebApp.Models;

public abstract class FormularioContatoViewModel {
    [Required(ErrorMessage = "O campo \"Nome\" é obrigatório")]
    [MinLength(2, ErrorMessage = "O campo \"Nome\" precisa de no mínimo 2 caracteres")]
    [MaxLength(100, ErrorMessage = "O campo \"Nome\" não pode exceder 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo \"Email\" é obrigatório")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "O campo \"Email\" deve conter um formato válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo \"Telefone\" é obrigatório")]
    [RegularExpression(@"^\(?\d{2}\)?\s?\d{4,5}-?\d{4}$", ErrorMessage = "O campo \"Telefone\" deve seguir o formato (00) 9 0000-0000 ou (00) 0000-0000")]
    public string Telefone { get; set; } = string.Empty;
    public string? Empresa { get; set; }
    public string? Cargo { get; set; }

    public void FormatarTelefone() {
        var numeros = new string(Telefone.Where(char.IsDigit).ToArray());

        if (numeros.Length == 11)
            Telefone = $"({numeros[..2]}) {numeros[2..7]}-{numeros[7..]}";
        else if (numeros.Length == 10)
            Telefone = $"({numeros[..2]}) 9{numeros[2..6]}-{numeros[6..]}";
    }
}

public class CadastrarContatoViewModel : FormularioContatoViewModel {
    public CadastrarContatoViewModel() { }
    public CadastrarContatoViewModel(string nome, string email, string telefone, string? empresa, string? cargo) : this() {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Cargo = cargo;
        Empresa = empresa;
    }
}

public class EditarContatoViewModel : FormularioContatoViewModel {
    public Guid Id { get; set; }
    public EditarContatoViewModel() { }
    public EditarContatoViewModel(Guid id, string nome, string email, string telefone, string? empresa, string? cargo) : this() {
        Id = id;
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Cargo = cargo;
        Empresa = empresa;
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
    public string? Empresa { get; set; }
    public string? Cargo { get; set; }

    public DetalhesContatoViewModel(Guid id, string nome, string email, string telefone, string? empresa, string? cargo) {
        Id = id;
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Empresa = empresa;
        Cargo = cargo;
    }
    public string TelefoneFormatado
    {
        get {
            var numeros = new string(Telefone.Where(char.IsDigit).ToArray());

            if (numeros.Length == 11)
                // 1,2   3,4,5,6,7   8,9,10,11
                return $"({numeros[..2]}) {numeros[2..7]}-{numeros[7..]}";
            if (numeros.Length == 10)
                // 1,2   3,4,5,6   7,8,9,10
                return $"({numeros[..2]}) 9{numeros[2..6]}-{numeros[6..]}";

            return Telefone;
        }
    }
}

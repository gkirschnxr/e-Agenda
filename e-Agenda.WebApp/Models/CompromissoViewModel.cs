using e_Agenda.Dominio.ModuloCompromissos;
using e_Agenda.Dominio.ModuloContato;
using e_Agenda.WebApp.Extensions;

namespace e_Agenda.WebApp.Models;

public class FormularioCompromissoViewModel {
    public string Assunto { get; set; } = string.Empty;
    public DateTime DataOcorrencia { get; set; }
    public DateTime HoraInicio { get; set; }
    public DateTime HoraTermino { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public Contato Contato { get; set; } = null!;
}

public class CadastrarCompromissoViewModel : FormularioCompromissoViewModel {
    public CadastrarCompromissoViewModel() { }

    public CadastrarCompromissoViewModel(string assunto, DateTime dataOcorrencia, DateTime horaInicio,
                                        DateTime horaTermino, string tipo, Contato contato) : this() {
        
        Assunto = assunto;
        DataOcorrencia = dataOcorrencia;
        HoraInicio = horaInicio;
        HoraTermino = horaTermino;
        Tipo = tipo;
        Contato = contato;
    }
}

public class VisualizarCompromissosViewModel {
    public List<DetalhesCompromissoViewModel> Registros { get; set; }

    public VisualizarCompromissosViewModel(List<Compromisso> compromissos) {
        Registros = [];

        foreach (var c in compromissos) {
            var detalhesVM = c.ParaDetalhesVM();

            if (detalhesVM != null)
                Registros.Add(detalhesVM);
        }
    }
}

public class DetalhesCompromissoViewModel {
    public Guid Id { get; set; }
    public string Assunto { get; set; } = string.Empty;
    public DateTime DataOcorrencia { get; set; }
    public DateTime HoraInicio { get; set; }
    public DateTime HoraTermino { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public Contato Contato { get; set; } = null!;

    public DetalhesCompromissoViewModel(Guid id, string assunto, DateTime dataOcorrencia, 
                                       DateTime horaInicio, DateTime horaTermino, string tipo, Contato contato) {
        Id = id;
        Assunto = assunto;
        DataOcorrencia = dataOcorrencia;
        HoraInicio = horaInicio;
        HoraTermino = horaTermino;
        Tipo = tipo;
        Contato = contato;
    }
}

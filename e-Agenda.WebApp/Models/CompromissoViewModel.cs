using e_Agenda.Dominio.ModuloCompromissos;
using e_Agenda.Dominio.ModuloContato;
using e_Agenda.WebApp.Extensions;
using System.ComponentModel.DataAnnotations;
using static e_Agenda.Dominio.ModuloCompromissos.Compromisso;

namespace e_Agenda.WebApp.Models;

public class FormularioCompromissoViewModel {
    public string Assunto { get; set; } = string.Empty;
    public DateOnly DataOcorrencia { get; set; }
    public DateTime HoraInicio { get; set; }
    public DateTime HoraTermino { get; set; }
    public TipoCompromisso Tipo { get; set; }

    [Display(Name = "Link da Reunião")]
    public string? Link { get; set; }

    [Display(Name = "Local")]
    public string? Local { get; set; }
}

public class CadastrarCompromissoViewModel : FormularioCompromissoViewModel {
    public CadastrarCompromissoViewModel() { }

    public CadastrarCompromissoViewModel(string assunto, DateOnly dataOcorrencia, DateTime horaInicio,
                                        DateTime horaTermino, TipoCompromisso tipo) : this() {
        
        Assunto = assunto;
        DataOcorrencia = dataOcorrencia;
        HoraInicio = horaInicio;
        HoraTermino = horaTermino;
        Tipo = tipo;
    }
}

public class EditarCompromissoViewModel : FormularioCompromissoViewModel
{
    public Guid Id { get; set; }
    public EditarCompromissoViewModel() { }
    public EditarCompromissoViewModel(Guid id, string assunto, DateOnly dataOcorrencia, DateTime horaInicio,
                                     DateTime horaTermino, TipoCompromisso tipo) : this() {
        Id = id;
        DataOcorrencia = dataOcorrencia;
        HoraInicio = horaInicio;
        HoraTermino = horaTermino;
        Tipo = tipo;
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
    public DateOnly DataOcorrencia { get; set; }
    public DateTime HoraInicio { get; set; }
    public DateTime HoraTermino { get; set; }
    public TipoCompromisso Tipo { get; set; }

    public DetalhesCompromissoViewModel(Guid id, string assunto, DateOnly dataOcorrencia, 
                                       DateTime horaInicio, DateTime horaTermino, TipoCompromisso tipo) {
        Id = id;
        Assunto = assunto;
        DataOcorrencia = dataOcorrencia;
        HoraInicio = horaInicio;
        HoraTermino = horaTermino;
        Tipo = tipo;
    }
}

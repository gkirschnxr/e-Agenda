using e_Agenda.Dominio.Compartilhado;
using e_Agenda.Dominio.ModuloContato;

namespace e_Agenda.Dominio.ModuloCompromissos;

public class Compromisso : EntidadeBase<Compromisso>
{
    public string Assunto { get; set; } = string.Empty;
    public DateOnly DataOcorrencia { get; set; }
    public DateTime HoraInicio { get; set; }
    public DateTime HoraTermino { get; set; }
    public enum TipoCompromisso {Presencial,Online}
    public TipoCompromisso Tipo { get; set; }
    public string? Link { get; set; }
    public string? Local { get; set; }

    public Compromisso() { }

    public Compromisso(string assunto, DateOnly dataOcorrencia, DateTime horaInicio, DateTime horaTermino, TipoCompromisso tipo) {
        Id = Guid.NewGuid();
        Assunto = assunto;
        DataOcorrencia = dataOcorrencia;
        HoraInicio = horaInicio;
        HoraTermino = horaTermino;
        Tipo = tipo;
    }

    public override void AtualizarRegistro(Compromisso registroEditado) {
        Assunto = registroEditado.Assunto;
        DataOcorrencia = registroEditado.DataOcorrencia;
        HoraInicio = registroEditado.HoraInicio;
        HoraTermino = registroEditado.HoraTermino;
        Tipo = registroEditado.Tipo;
    }
}
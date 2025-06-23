using e_Agenda.Dominio.Compartilhado;
using e_Agenda.Dominio.ModuloContato;

namespace e_Agenda.Dominio.ModuloCompromissos;

public class Compromisso : EntidadeBase<Compromisso>
{
    public string Assunto { get; set; } = string.Empty;
    public DateTime DataOcorrencia { get; set; }
    public DateTime HoraInicio { get; set; }
    public DateTime HoraTermino { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public Contato Contato { get; set; } = null!;

    public Compromisso() { }
    public Compromisso(string assunto, DateTime dataOcorrencia, DateTime horaInicio, DateTime horaTermino, string tipo, Contato contato) {
        Assunto = assunto;
        DataOcorrencia = dataOcorrencia;
        HoraInicio = horaInicio;
        HoraTermino = horaTermino;
        Tipo = tipo;
        Contato = contato;
    }

    public override void AtualizarRegistro(Compromisso registroEditado) {
        Assunto = registroEditado.Assunto;
        DataOcorrencia = registroEditado.DataOcorrencia;
        HoraInicio = registroEditado.HoraInicio;
        HoraTermino = registroEditado.HoraTermino;
        Tipo = registroEditado.Tipo;
        Contato = registroEditado.Contato;
    }
}
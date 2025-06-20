using e_Agenda.Dominio.ModuloCompromissos;
using e_Agenda.WebApp.Models;

namespace e_Agenda.WebApp.Extensions;

public static class CompromissoExtensions
{
    public static DetalhesCompromissoViewModel ParaDetalhesVM(this Compromisso compromisso) {
        return new DetalhesCompromissoViewModel(compromisso.Id, compromisso.Assunto, compromisso.DataOcorrencia, compromisso.HoraInicio,
                                           compromisso.HoraTermino, compromisso.Tipo, compromisso.Contato);
    }

}

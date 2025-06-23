using e_Agenda.Dominio.Compartilhado;

namespace e_Agenda.Dominio.ModuloContato;

public class Contato : EntidadeBase<Contato>
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;

    //public string Cargo { get; set; }
    //public string Empresa { get; set; }
    public Contato() { }
    public Contato(string nome, string email, string telefone) {
        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        Telefone = telefone;
    }

    public override void AtualizarRegistro(Contato registroEditado) {
        Nome = registroEditado.Nome;
        Email = registroEditado.Email;
        Telefone= registroEditado.Telefone;
    }
}
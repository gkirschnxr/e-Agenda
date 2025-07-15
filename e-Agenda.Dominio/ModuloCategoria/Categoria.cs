﻿using e_Agenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloDespesa;

namespace eAgenda.Dominio.ModuloCategoria;

public class Categoria : EntidadeBase<Categoria>
{
    public string Titulo { get; set; } = string.Empty;
    public List<Despesa> Despesas { get; set; }

    public Categoria()
    {
        Despesas = new List<Despesa>();
    }

    public Categoria(string titulo) : this()
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
    }

    public override void AtualizarRegistro(Categoria registroEditado)
    {
        Titulo = registroEditado.Titulo;
    }
}
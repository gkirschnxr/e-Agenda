﻿using e_Agenda.Dominio.ModuloTarefa;
using e_Agenda.WebApp.Extensions;
using e_Agenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace e_Agenda.WebApp.Models
{
    public class FormularioTarefaViewModel
    {
        [Required(ErrorMessage = "O campo \"Titulo\" é obrigatório.")]
        [MinLength(2, ErrorMessage = "O campo \"Titulo\" precisa conter ao menos 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "O campo \"Titulo\" precisa conter no máximo 100 caracteres.")]
        public string Titulo { get; set; }

        public PrioridadeTarefa Prioridade { get; set; }
    }

    public class CadastrarTarefaViewModel : FormularioTarefaViewModel
    {
        public CadastrarTarefaViewModel() { }

        public CadastrarTarefaViewModel(string titulo, PrioridadeTarefa prioridade)
        {
            Titulo = titulo;
            Prioridade = prioridade;
        }
    }

    public class EditarTarefaViewModel : FormularioTarefaViewModel
    {
        public Guid Id { get; set; }

        public EditarTarefaViewModel() { }

        public EditarTarefaViewModel(Guid id, string titulo, PrioridadeTarefa prioridade)
        {
            Id = id;
            Titulo = titulo;
            Prioridade = prioridade;
        }
    }

    public class ExcluirTarefaViewModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }

        public ExcluirTarefaViewModel(Guid id, string titulo)
        {
            Id = id;
            Titulo = titulo;
        }
    }


    public class VisualizarTarefaViewModel
    {
        public List<DetalhesTarefaViewModel> Registros { get; set; }

        public VisualizarTarefaViewModel(List<Tarefa> tarefas)
        {
            Registros = new List<DetalhesTarefaViewModel>();

            foreach (var t in tarefas)
                Registros.Add(t.ParaDetalhesVM());
        }
    }

    public class DetalhesTarefaViewModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public PrioridadeTarefa Prioridade { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public decimal Percentual { get; set; }
        public bool Concluido { get; set; }
        
        public DetalhesTarefaViewModel(
            Guid id, 
            string titulo, 
            PrioridadeTarefa prioridade, 
            DateTime criacao, 
            DateTime? conclusao, 
            decimal percentual, 
            bool concluido
            
            )
        {
            Id = id;
            Titulo = titulo;
            Prioridade = prioridade;
            DataCriacao = criacao;
            DataConclusao = conclusao;
            Percentual = percentual;
            Concluido = concluido;

            }
        }
    

    public class ItemTarefaViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public bool Concluido { get; set; }

        public ItemTarefaViewModel(Guid id, string nome, bool concluido)
        {
            Id = id;
            Nome = nome;
            Concluido = concluido;
        }
    }

    public class GerenciarItensTarefaViewModel
    {
        public DetalhesTarefaViewModel Tarefa { get; set; }
        public List<ItemTarefaViewModel> Itens { get; set; }

        public GerenciarItensTarefaViewModel(){}

        public GerenciarItensTarefaViewModel(Tarefa tarefa) : this()
        {
            Tarefa = tarefa.ParaDetalhesVM();

            Itens = new List<ItemTarefaViewModel>();

            foreach (var i in tarefa.Itens)
            {
                var itemVM = new ItemTarefaViewModel(i.Id,i.Titulo, i.Concluido);

                Itens.Add(itemVM);
            }
        }
    }


}



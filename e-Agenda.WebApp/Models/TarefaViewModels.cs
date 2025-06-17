using e_Agenda.Dominio.ModuloTarefa;
using e_Agenda.WebApp.Extensions;
using System.ComponentModel.DataAnnotations;

namespace e_Agenda.WebApp.Models
{
    public class FormularioTarefaViewModels
    {
        [Required(ErrorMessage = "O campo \"Titulo\" é obrigatório.")]
        [MinLength(2, ErrorMessage = "O campo \"Titulo\" precisa conter ao menos 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "O campo \"Titulo\" precisa conter no máximo 100 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "O campo \"Prioridade\" é obrigatório")]
        public string Prioridade { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataConclusao { get; set; }
        public bool Status = false;
        public double Percentual { get; set; }
        public string ItensTarefa { get; set; }
    }

    public class CadastrarTarefaViewModel : FormularioTarefaViewModels 
    {
        public CadastrarTarefaViewModel(){}

        public CadastrarTarefaViewModel(
            string titulo, 
            string prioridade, 
            DateTime dataCriacao,
            DateTime dataConclusao,
            bool status,
            double percentual,
            string itensTarefa
            ) : this()
        {
            Titulo = titulo;
            Prioridade = prioridade;
            DataCriacao = dataCriacao;
            DataConclusao = dataConclusao;
            Status = status;
            Percentual = percentual;
            ItensTarefa = itensTarefa;
        }
    }

    public class EditarTarefaViewModel : FormularioTarefaViewModels 
    {
        public Guid Id { get; set; }

        public EditarTarefaViewModel(){}

        public EditarTarefaViewModel(Guid id,string titulo, string prioridade, string itensTarefa)
        {
            Id = id;
            Titulo = titulo;
            Prioridade = prioridade;
            ItensTarefa = itensTarefa;
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

    public class VisualizarTarefasViewModel 
    {
        public List<DetalhesTarefasViewModel> Registros { get; set; }

        public VisualizarTarefasViewModel(List<Tarefa> tarefas) 
        {
            Registros = new List<DetalhesTarefasViewModel>();

            foreach (var t in tarefas)
                Registros.Add(t.ParaDetalhesVM());
            
        }
    }

    public class DetalhesTarefasViewModel 
    {
        public Guid Id{ get; set; }
        public string Titulo { get; set; }
        public string Prioridade { get; set; }
        public string ItensTarefa { get; set; }

        public DetalhesTarefasViewModel(Guid id, string titulo, string prioridade, string itensTarefa)
        {
            Id = id;
            Titulo = titulo;
            Prioridade = prioridade;
            ItensTarefa = itensTarefa;
        }

    }
}

using e_Agenda.Dominio.Compartilhado;
using System.ComponentModel.DataAnnotations;

namespace e_Agenda.Dominio.ModuloTarefa
{
    public class Tarefa : EntidadeBase<Tarefa>
    {
        public string Titulo { get; set; }
        public PrioridadeTarefa Prioridade { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public List<ItemTarefa> Itens { get; set; }

        public decimal PercentualConcluido
        {
            get
            {
                if (Itens == null || Itens.Count == 0)
                    return default;

                int qtdConcluidos = 0;

                foreach (var item in Itens)
                {
                    if (item.Concluido)
                        qtdConcluidos++;
                }

                decimal percentualBase = qtdConcluidos / (decimal)Itens.Count * 100;

                return Math.Round(percentualBase, 2);
            }
        }

        public bool Concluida { get; set; }

        public Tarefa()
        {
            Itens = new List<ItemTarefa>();
        }

        public Tarefa(string titulo, PrioridadeTarefa prioridade)
        {
            Id = Guid.NewGuid();
            Titulo = titulo;
            Prioridade = prioridade;
            Concluida = false;
            DataCriacao = DateTime.Now;
        }

        public void ConcluirTarefa() 
        {
            Concluida = true;
            DataConclusao = DateTime.Now;
        }

        public void MarcarPendente() 
        {
            Concluida = false;
            DataConclusao = null;
        }

        public ItemTarefa? ObterItem(Guid idItem) 
        {
            foreach (var i in Itens)
            {
                if (idItem.Equals(i.Id))
                    return i;
            }

            return null;
        }

        public bool AdicionarItem(string titulo)
        {
            var item = new ItemTarefa(titulo);

            foreach (var i in Itens)
            {
                if (item.Id == i.Id)
                    return false;
            }

            Itens.Add(item);

            MarcarPendente();

            return true;
        }

        public ItemTarefa RemoverItem(ItemTarefa item)
        {
            Itens.Remove(item);

            MarcarPendente();

            return item;
        }

        public void ConcluirItem(ItemTarefa item)
        {
            item.Concluir();
        }

        public void MarcarItemPendente(ItemTarefa item)
        {
            item.MarcarPendente();

            MarcarPendente();
        }


        public override void AtualizarRegistro(Tarefa registroEditado)
        {
            Titulo = registroEditado.Titulo;
            Prioridade = registroEditado.Prioridade;

        }

        
    }
}

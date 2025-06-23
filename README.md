![](https://imgur.com/hg2U6Lg.gif)

# 📅 e-Agenda
Sistema de gerenciamento de contatos, compromissos, categorias, despesas e tarefas.

## 📝 Sobre o Projeto
O e-Agenda é uma aplicação desenvolvida em C# com ASP.NET MVC, seguindo o padrão 3 camadas (Apresentação, Domínio e Infraestrutura) e utilizando conceitos de Programação Orientada a Objetos (POO).

Este projeto foi proposto pela Academia do Programador 2025 e desenvolvido com foco em boas práticas, validação de dados e organização de código.

## 🚀 Funcionalidades
### 📇 Módulo de Contatos
- Inserção, edição, exclusão e visualização de contatos

- Validação de:

  - Nome (2-100 caracteres)

  - Email (formato válido)

  - Telefone (formatos: (XX) XXXX-XXXX ou (XX) XXXXX-XXXX)

- Restrições:

  - Não permitido contatos com email ou telefone duplicados

  - Não permitido exclusão de contatos com compromissos vinculados


### 📅 Módulo de Compromissos
- Cadastro, edição, exclusão e visualização de compromissos

- Tipos: Presencial (campo Local) ou Remoto (campo Link)

- Validações de:

  - Assunto (2-100 caracteres)

  - Datas e horários

  - Conflito de horários
    

### 🏷️ Módulo de Categorias
- Gerenciamento de categorias (CRUD)

- Visualização de despesas por categoria

- Validação de título único

- Bloqueio de exclusão se houver despesas vinculadas


### 💸 Módulo de Despesas
- CRUD de despesas

- Campos:

  - Descrição

  - Valor

  - Forma de pagamento (À vista, Crédito, Débito)

  - Categoria(s)


### ✅ Módulo de Tarefas
- CRUD de tarefas e visualização por prioridade/status

- Controle de percentual de conclusão

- Inclusão de itens da tarefa

- Campos obrigatórios como Título, Prioridade, Data de Criação/Conclusão e Status


### 🖥️ Tecnologias Utilizadas
![My Skills](https://skillicons.dev/icons?i=cs,bootstrap,html,css,js,dotnet)

### Requisitos
- .NET SDK (recomendado .NET 8.0 ou superior) para compilação e execução do projeto.

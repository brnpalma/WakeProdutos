# WakeProdutos API

Aplicação para gestão de produtos (CRUD) desenvolvida em .NET 9, usando arquitetura em camadas (Clean Architecture) e padrões de projeto como Repository, CQRS e Unit of Work (via EF Core). A API expõe endpoints para criar, atualizar, listar, obter por id e deletar produtos.

## 🛠️ Setup automático

Ao executar a aplicação, o banco de dados e suas tabelas são **criados automaticamente** via Entity Framework Core. Não é necessário rodar comandos manuais — as migrações são aplicadas na inicialização, facilitando o processo de clonagem e execução do projeto sem esforço adicional, já que este projeto é destinado apenas a testes em ambiente de desenvolvimento.

## 💡 Observações iniciais

- O modo *Code-First* foi o escolhido para desenvolvimento de toda a aplicação.
- Por padrão, inicializa com *Scalar* (`Scalar.AspNetCore`) — uma interface mais completa e com visual personalizado em `https://localhost:{port}/scalar`.
- Também é possível usar o *Swagger* (via `Swashbuckle`) se preferir uma interface mais tradicional em `https://localhost:{port}/swagger`.

## 🗂️ Projetos na Solution

- `WakeProdutos.API` — Projeto ASP.NET Core Web API com controllers e configuração do pipeline.
- `WakeProdutos.Application` — Casos de uso, DTOs e handlers (MediatR) para CQRS.
- `WakeProdutos.Infrastructure` — Implementações de acesso a dados, EF Core `DbContext`, repositórios e seed.
- `WakeProdutos.Domain` — Entidades e regras de negócio.
- `WakeProdutos.Shared` — Constantes, classes de resultados genéricos (Result<T>) e utilitários compartilhados.
- `tests/WakeProdutos.Tests.Unit` — Testes unitários (xUnit, Moq, FluentAssertions).
- `tests/WakeProdutos.Tests.Integration` — Testes de integração (xUnit, WebApplicationFactory, InMemory DB).

## 📐 Padrões de projeto utilizados

- Repository: `IProdutoRepository` e `ProdutoRepository` isolam acesso ao banco.
- Unit of Work: usado implicitamente via `WakeDbContext` do EF Core (chamadas a `SaveChangesAsync` concentram a persistência).
- CQRS: separação entre Commands (alterações) e Queries (consultas) usando MediatR.
- Mediator: `MediatR` para mediar chamadas entre controllers e handlers.

## 🧩 Requisitos e Tecnologias Principais  

- 🖥️ .NET 9.0 (C# 13)
- 🌐 ASP.NET Core Web API
- 🗄️ Entity Framework Core
- 🧪 Microsoft.EntityFrameworkCore.InMemory (para testes de integração)
- 📬 MediatR
- 📘 Scalar.AspNetCore (UI alternativa para documentação)
- 📚 Swashbuckle / Swagger
- 🧷 xUnit
- 🎭 Moq
- 🔍 FluentAssertions

## 🗄️  Abordagem do Entity Framework

- As entidades são definidas no projeto `Domain` e o `WakeDbContext` mapeia estas entidades.
- Para os testes de integração usamos `InMemoryDatabase` e `WakeDbContextSeed` para popular dados com 5 produtos iniciais.

## ▶️ Como executar o projeto

1. Iniciar API localmente:
 - A partir da pasta raiz da solution: `dotnet run --project WakeProdutos/WakeProdutos.API.csproj`
 - Por padrão a aplicação inicia com a interface `Scalar`.
2. Usar Scalar (padrão):
 - Acesse `https://localhost:{port}/scalar` para usar a UI do Scalar.
3. Ou acessar a interface Swagger (opcional):
 - Acesse `https://localhost:{port}/swagger` para a UI tradicional opcionalmente.

 OBS.: O padrão de portas do projeto é, 7284 (HTTPS) e 5091 (HTTP), porém, confira no console a porta gerada após execução de `dotnet run`.

## 🧪 Como executar os testes

- Testes unitários: `dotnet test tests/WakeProdutos.Tests.Unit`
- Testes de integração: `dotnet test tests/WakeProdutos.Tests.Integration`

## 🧪 Descrição dos Testes

O repositório contém testes unitários e de integração. Abaixo uma lista dos testes atuais e uma breve descrição de cada um.

Testes unitários (pasta: `tests/WakeProdutos.Tests.Unit`)
- `ProdutoTests/CadastrarProdutoTests.cs` — Valida cenários do comando de cadastro: sucesso com dados válidos, erros para nome inválido e valor negativo.
- `ProdutoTests/AtualizarProdutoTests.cs` — Testa atualização de produto: sucesso para produto existente com dados válidos, 404 para produto inexistente e 400 para dados inválidos.
- `ProdutoTests/DeletarProdutoTests.cs` — Verifica o comportamento do comando de exclusão lógica: sucesso ao deletar produto existente e 404 para produto inexistente.
- `ProdutoTests/ListarProdutosTests.cs` — Testa a query de listagem: retorna coleção completa sem filtros e valida comportamento ao receber parâmetro `ordenarPor` inválido.
- `ProdutoTests/ObterProdutoPorIdTests.cs` — Testa a query de obter por id: retorna produto específico quando existe e 404 quando não existe.

Testes de integração (pasta: `tests/WakeProdutos.Tests.Integration`)
- `IntegrationTestsFactory.cs` — Fabrica de testes que configura o ambiente de teste (WebApplicationFactory) usando um banco InMemory e popula dados via `WakeDbContextSeed`.
- `ProdutosControllerTests.cs` — Testes de API end-to-end contra a aplicação em memória.
 - `ListarProdutos_RetornaProdutosSeedados` — Verifica listagem de produtos seedados.
 - `ObterProdutoPorId_RetornaProduto` — Valida retorno de produto por id existente.
 - `CadastrarProduto_CriaProduto` — Testa endpoint de criação e código 201 de retorno.
 - `AtualizarProduto_AtualizaProduto` — Cria um produto e testa atualização via PUT.
 - `DeletarProduto_ExcluiProdutoLogicamente` — Cria e deleta um produto. Valida se a exclusão é feita logicamente (GET por id retorna 404 e o produto não aparece mais na listagem).

## 🔗 Endpoints principais

Base: `/api/v1`
- `GET /produtos` — Listar produtos (opções de filtro por nome e ordenação por Estoque, Valor, Id ou Nome)
- `GET /produtos/{id}` — Obter produto por id
- `POST /produtos` — Cadastrar produto
- `PUT /produtos/{id}` — Atualizar produto
- `DELETE /produtos/{id}` — Deletar produto. Aqui ocorre uma exclusão lógica (soft delete) para maior rastreabilidade e consistência dos dados.

## 📁 Estrutura da Solution (principais pastas/arquivos)

```
📁 src
├─📁 WakeProdutos.API
│  ├─📁 Properties
│  │  └─📄 launchSettings.json
│  ├─📁 Controllers
│  │  └─📄 ProdutoController.cs
│  ├─📁 Extensions
│  │  └─📄 DependencyInjection.cs
│  ├─📁 Filters
│  │  └─📄 ValidationFilterAttribute.cs
│  ├─📁 Middleware
│  │  └─📄 ExceptionMiddleware.cs
│  ├─📄 appsettings.json
│  ├─📄 Program.cs
│  └─📄 WakeProdutos.http
│
├─📁 WakeProdutos.Application
│  ├─📁 Dtos
│  │  ├─📄 AtualizarProdutoDto.cs
│  │  ├─📄 ListaProdutoDto.cs
│  │  └─📄 ProdutoDto.cs
│  ├─📁 UseCases
│  │  ├─📁 Produtos
│  │  │  ├─📁 Commands
│  │  │  │  ├─📄 AtualizarProdutoCommand.cs
│  │  │  │  ├─📄 CadastrarProdutoCommand.cs
│  │  │  │  └─📄 DeletarProdutoCommand.cs
│  │  │  ├─📁 Handlers
│  │  │  │  ├─📄 AtualizarProdutoHandler.cs
│  │  │  │  ├─📄 CadastrarProdutoHandler.cs
│  │  │  │  └─📄 DeletarProdutoHandler.cs
│  │  │  ├─📁 Queries
│  │  │  │  ├─📄 ListarProdutosHandler.cs
│  │  │  │  ├─📄 ListarProdutosQuery.cs
│  │  │  │  ├─📄 ObterProdutoPorIdQuery.cs
│  │  │  │  └─📄 ObterProdutoPorIdQueryHandler.cs
│  └─📄 DependencyInjection.cs
│
├─📁 WakeProdutos.Domain
│  ├─📁 Entities
│  │  └─📄 Produto.cs
│  └─📁 Interfaces
│     └─📄 IProdutoRepository.cs
│
├─📁 WakeProdutos.Infrastructure
│  └─📁 Data
│     ├─📁 Context
│     │  ├─📄 WakeDbContext.cs
│     │  └─📄 WakeDbContextSeed.cs
│     ├─📁 Repositories
│     │  └─📄 ProdutoRepository.cs
│     └─📄 DependencyInjection.cs
│
└─📁 WakeProdutos.Shared
   ├─📁 Constants
   │  └─📄 Constantes.cs
   └─📁 Results
      └─📄 Result.cs
   
📁 tests
├─📁 WakeProdutos.Tests.Integration
│  ├─📄 IntegrationTestsFactory.cs
│  └─📄 ProdutosControllerTests.cs
│
└─📁 WakeProdutos.Tests.Unit
   └─📁 ProdutoTests
      ├─📄 AtualizarProdutoTests.cs
      ├─📄 CadastrarProdutoTests.cs
      ├─📄 DeletarProdutoTests.cs
      ├─📄 ListarProdutosTests.cs
      └─📄 ObterProdutoPorIdTests.cs
```

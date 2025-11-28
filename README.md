# WakeProdutos

### Breve descrição
Aplicação de exemplo para gestão de produtos (CRUD) desenvolvida em .NET 9, usando arquitetura em camadas (Clean Architecture) e padrões de projeto como Repository, CQRS e Unit of Work (via EF Core). A API expõe endpoints para criar, atualizar, listar, obter por id e deletar produtos.

### Observações iniciais

- O modo *Code-First* foi o escolhido para desenvolvimento de toda a aplicação.
- Por padrão a aplicação inicializa com *Scalar* (`Scalar.AspNetCore`) — uma interface mais completa e com visual personalizado em `https://localhost:{port}/scalar`.
- Também é possível expor e usar o *Swagger* (via `Swashbuckle`) se preferir uma interface mais tradicional em `https://localhost:{port}/swagger`.

### Projetos na Solution

- `WakeProdutos.API` — Projeto ASP.NET Core Web API com controllers e configuração do pipeline.
- `WakeProdutos.Application` — Casos de uso, DTOs e handlers (MediatR) para CQRS.
- `WakeProdutos.Infrastructure` — Implementações de acesso a dados, EF Core `DbContext`, repositórios e seed.
- `WakeProdutos.Domain` — Entidades e regras de negócio.
- `WakeProdutos.Shared` — Constantes, resultados (Result<T>) genéricos e utilitários compartilhados.
- `tests/WakeProdutos.Tests.Unit` — Testes unitários (xUnit, Moq, FluentAssertions).
- `tests/WakeProdutos.Tests.Integration` — Testes de integração (xUnit, WebApplicationFactory, InMemory DB).

### Padrões de projeto utilizados

- Repository: `IProdutoRepository` e `ProdutoRepository` isolam acesso ao banco.
- Unit of Work: usado implicitamente via `WakeDbContext` do EF Core (chamadas a `SaveChangesAsync` concentram a persistência).
- CQRS: separação entre Commands (alterações) e Queries (consultas) usando MediatR.
- Mediator: `MediatR` para mediar chamadas entre controllers e handlers.

### Tecnologias e bibliotecas

- .NET9 (C#13)
- ASP.NET Core Web API
- Entity Framework Core (Code-First)
- Microsoft.EntityFrameworkCore.InMemory (para testes de integração)
- MediatR
- Scalar.AspNetCore (UI alternativa)
- Swashbuckle (Swagger)
- xUnit, Moq, FluentAssertions (testes)

### Estrutura da Solution (principais pastas/arquivos)

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

### Abordagem do Entity Framework

- As entidades são definidas no projeto `Domain` e o `WakeDbContext` mapeia estas entidades.
- Para os testes de integração usamos `InMemoryDatabase` e `WakeDbContextSeed` para popular dados com 5 produtos iniciais.

### Como executar o projeto

Pré-requisitos: .NET9 SDK instalado.

1. Iniciar API localmente:
 - A partir da pasta raiz da solution: `dotnet run --project WakeProdutos/WakeProdutos.API.csproj`
 - Por padrão a aplicação inicia com a interface `Scalar`.
2. Usar Scalar (padrão):
 - Acesse `https://localhost:{port}/scalar` para usar a UI do Scalar.
3. Ou acessar a interface Swagger (opcional):
 - Acesse `https://localhost:{port}/swagger` para a UI tradicional opcionalmente.

### Como executar os testes

- Testes unitários: `dotnet test tests/WakeProdutos.Tests.Unit`
- Testes de integração: `dotnet test tests/WakeProdutos.Tests.Integration`

### Endpoints principais

Base: `/api/v1`
- `GET /produtos` — Listar produtos (opções de filtro por nome e ordenação)
- `GET /produtos/{id}` — Obter produto por id
- `POST /produtos` — Cadastrar produto
- `PUT /produtos/{id}` — Atualizar produto
- `DELETE /produtos/{id}` — Deletar produto

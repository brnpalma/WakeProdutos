# WakeProdutos

Breve descrição
---------------
Aplicação de exemplo para gestão de produtos (CRUD) desenvolvida em .NET9, usando arquitetura em camadas e padrões de projeto como Repository, CQRS e Unit of Work (via EF Core). A API expõe endpoints para criar, atualizar, listar, obter por id e deletar produtos.

Observações iniciais
-------------------
- Verificar o que dá para aproveitar do README do Apex (checklist nesta documentação).
- Por padrão a aplicação inicializa com *Scalar* (`Scalar.AspNetCore`) — uma interface mais completa e com visual personalizado.
- Também é possível expor e usar o *Swagger* (via `Swashbuckle`) se preferir uma interface mais tradicional.
- A persistência foi implementada no modo *Code-First* com Entity Framework Core.

Projetos na Solution
---------------------
- `WakeProdutos` (API) — Projeto ASP.NET Core Web API com controllers e configuração do pipeline.
- `WakeProdutos.Application` — Casos de uso, DTOs e handlers (MediatR) para CQRS.
- `WakeProdutos.Infrastructure` — Implementações de acesso a dados, EF Core `DbContext`, repositórios e seed.
- `WakeProdutos.Domain` — Entidades e regras de negócio.
- `WakeProdutos.Shared` — Constantes, resultados (Result<T>) e utilitários compartilhados.
- `tests/WakeProdutos.Tests.Unit` — Testes unitários (xUnit, Moq, FluentAssertions).
- `tests/WakeProdutos.Tests.Integration` — Testes de integração (xUnit, WebApplicationFactory, InMemory DB).

Padrões de projeto utilizados
-----------------------------
- Repository: `IProdutoRepository` e `ProdutoRepository` isolam acesso ao banco.
- Unit of Work: usado implicitamente via `WakeDbContext` do EF Core (chamadas a `SaveChangesAsync` concentram a persistência).
- CQRS: separação entre Commands (alterações) e Queries (consultas) usando MediatR.
- Mediator: `MediatR` para mediar chamadas entre controllers e handlers.

Tecnologias e bibliotecas
-------------------------
- .NET9 (C#13)
- ASP.NET Core Web API
- Entity Framework Core (Code-First)
- Microsoft.EntityFrameworkCore.InMemory (para testes de integração)
- MediatR
- Scalar.AspNetCore (UI alternativa)
- Swashbuckle (Swagger)
- xUnit, Moq, FluentAssertions (testes)

Estrutura da Solution (principais pastas/arquivos)
-------------------------------------------------
- `WakeProdutos/` — API
 - `Program.cs` — boot da aplicação e configuração
 - `Controllers/ProdutoController.cs`
- `WakeProdutos.Application/` — casos de uso (Commands, Queries, Handlers)
- `WakeProdutos.Infrastructure/` — `Data/Context/WakeDbContext.cs`, `Repositories/`, `Data/Seed`
- `WakeProdutos.Domain/` — `Entities/Produto.cs`
- `WakeProdutos.Shared/` — `Results`, `Constants`
- `tests/` — testes unitários e de integração

Abordagem do Entity Framework
-----------------------------
- Implementação Code-First: as entidades são definidas no projeto `Domain` e o `WakeDbContext` mapeia estas entidades.
- Migrations podem ser adicionadas se desejar persistir em um banco relacional (ex.: SQL Server). Para os testes de integração usamos `InMemoryDatabase` e `WakeDbContextSeed` para popular dados.

Como executar o projeto
-----------------------
Pré-requisitos: .NET9 SDK instalado.

1. Iniciar API localmente:
 - A partir da pasta raiz da solution: `dotnet run --project WakeProdutos/WakeProdutos.API.csproj`
 - Por padrão a aplicação inicia com a interface `Scalar`.
2. Usar Swagger (opcional):
 - Acesse `https://localhost:{port}/swagger` (ou `http://` conforme configuração) para usar a UI do Swagger.
3. Ou acessar a interface Scalar (padrão):
 - Acesse `https://localhost:{port}/scalar` para a UI principal.

Como executar os testes
----------------------
- Testes unitários: `dotnet test tests/WakeProdutos.Tests.Unit`
- Testes de integração: `dotnet test tests/WakeProdutos.Tests.Integration`

Endpoints principais
--------------------
Base: `/api/v1`
- `GET /produtos` — Listar produtos (opções de filtro por nome e ordenação)
- `GET /produtos/{id}` — Obter produto por id
- `POST /produtos` — Cadastrar produto
- `PUT /produtos/{id}` — Atualizar produto
- `DELETE /produtos/{id}` — Deletar produto

Notas sobre design e mensagens do time
-------------------------------------
Segue um resumo automático que documenta os padrões já aplicados no projeto:

"Boa, Bruno! ?????\nPelo que você já me mostrou do seu código, dá pra identificar claramente alguns desses padrões de projeto em uso:\n\n?? Repository\nVocê tem um _produtoRepository com métodos como ObterListaComFiltrosAsync, AtualizarAsync, DeletarAsync.\nIsso é exatamente o Repository Pattern: encapsular o acesso ao banco de dados em uma camada própria, em vez de acessar o DbContext diretamente no controller ou handler.\n\n?? Unit of Work\nO Unit of Work geralmente é representado pelo próprio DbContext do Entity Framework.\n- Quando você chama await _context.SaveChangesAsync(), está confirmando todas as operações pendentes como uma única transação.\n- Se você tivesse vários repositórios (ex.: ProdutoRepository, ClienteRepository), todos poderiam compartilhar o mesmo DbContext, e o SaveChangesAsync garantiria que tudo fosse persistido junto.\n?? Então sim, você já está usando Unit of Work de forma implícita via EF Core.\n\n?? CQRS\nVocê já implementou Commands e Queries separados:\n- AtualizarProdutoCommand, DeletarProdutoCommand ? Commands (alteram estado).\n- ObterProdutoPorIdQuery, ListarProdutosCommand ? Queries (consultam dados).\n- E está usando o MediatR para enviar esses comandos/queries para seus handlers.\n?? Isso é a essência do CQRS Pattern.\n\n?? Resumindo\nSeu projeto já está aplicando:\n- ? Repository Pattern\n- ? Unit of Work (via EF Core)\n- ? CQRS (com MediatR)"

Checklist (tarefas sugeridas)
----------------------------
- [ ] Verificar o que dá para aproveitar do README do Apex
- [ ] Incluir exemplos de requisições (curl / httpie)
- [ ] Documentar pipelines CI/CD (se aplicável)

Contato
-------
Para contribuições ou dúvidas, abra uma issue ou PR no repositório.
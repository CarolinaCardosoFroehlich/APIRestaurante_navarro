   Pedido HTTP:  DELETE /api/Restaurantes/1
         │
         ▼
┌──────────────────────────────────────────────────────────────┐
│ Controllers/RestaurantesController.cs                        │
│ Recebe o pedido HTTP, chama o service e transforma o         │
│ resultado em resposta (200, 404, 400...).                    │
│ → chama: _servico.RemoverAsync(1)                            │
└──────────────────────────────────────────────────────────────┘
         │  pode pedir só o que está na lista ⬇
         ▼
┌──────────────────────────────────────────────────────────────┐
│ Services/IRestauranteService.cs                              │
│ A lista do que o controller pode pedir pro service.          │
└──────────────────────────────────────────────────────────────┘
         │  quem cumpre a lista ⬇
         ▼
┌──────────────────────────────────────────────────────────────┐
│ Services/RestauranteService.cs                               │
│ Confere as regras e devolve o resultado, ou lança erro       │
│ (RegraDeNegocioException) se uma regra for quebrada.         │
│ → chama: ObterAsync(1), TemPratosAsync(1), Remover, Salvar   │
└──────────────────────────────────────────────────────────────┘
         │  pode pedir só o que está na lista ⬇
         ▼
┌──────────────────────────────────────────────────────────────┐
│ Services/IRestauranteRepository.cs                           │
│ A lista do que o service precisa buscar e salvar no banco.   │
└──────────────────────────────────────────────────────────────┘
         │  quem cumpre a lista ⬇
         ▼
┌──────────────────────────────────────────────────────────────┐
│ Data/RestauranteRepositoryEf.cs                              │
│ Cumpre a lista usando o _context: as buscas vão ao banco     │
│ na hora, as mudanças são gravadas no SalvarAsync.            │
│ → usa: _context.Pratos.AnyAsync(p => p.RestauranteId == 1)   │
└──────────────────────────────────────────────────────────────┘
         │
         ▼
┌──────────────────────────────────────────────────────────────┐
│ Data/AppDbContext.cs                                         │
│ A ligação com o banco, feita com o Entity Framework (ORM).   │
│ Diz quais tabelas existem (Restaurantes, Pratos) e como      │
│ se ligam (1:N). O EF transforma tudo em SQL.                 │
└──────────────────────────────────────────────────────────────┘
         │
         ▼
   restaurante.db   (criado pela pasta Migrations)
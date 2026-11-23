using APIRestaurante.Data;
using APIRestaurante.Services;        
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registra os controllers (as classes que recebem as requisições HTTP)
builder.Services.AddControllers();

// Registra o AppDbContext e diz que o banco é SQLite,
// usando a connection string do appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// NOVO: repositórios — quando pedirem o contrato, entregue a versão com EF
builder.Services.AddScoped<IRestauranteRepository, RestauranteRepositoryEf>();
builder.Services.AddScoped<IPratoRepository, PratoRepositoryEf>();

// NOVO: services — quando pedirem o contrato, entregue o service com as regras
builder.Services.AddScoped<IRestauranteService, RestauranteService>();
builder.Services.AddScoped<IPratoService, PratoService>();

// Swagger: gera a documentação da API e uma página para testar os endpoints
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Em desenvolvimento, habilita a página do Swagger em /swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
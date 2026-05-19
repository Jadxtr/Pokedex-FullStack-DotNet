var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. CONFIGURAÇÃO DOS SERVIÇOS (DI)
// ==========================================

builder.Services.AddControllers();

// 💡 ATIVA O HTTPCLIENT (A linha que faltava para o seu PokemonService funcionar!)
builder.Services.AddHttpClient();

// Registro do seu serviço usando o caminho completo
builder.Services.AddScoped<PokedexAPI.Services.PokemonService>();

// Configuração do CORS para liberar o projeto Angular rodar livremente
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ==========================================
// 2. MIDDLEWARES / PIPELINE
// ==========================================

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
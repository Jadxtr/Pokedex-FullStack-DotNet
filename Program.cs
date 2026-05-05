using PokedexAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. ADICIONE ISTO: Suporte para Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 2. ADICIONE ISTO: Configuração do HttpClient e do seu Serviço
builder.Services.AddHttpClient<PokemonService>();
builder.Services.AddScoped<PokemonService>();

// 3. ADICIONE ISTO: Liberar o acesso para o seu index.html (CORS)
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configurações de ambiente
if (app.Environment.IsDevelopment())
{
    // O template novo usa OpenAPI em vez de Swagger por padrão
    app.MapOpenApi();
}

// 4. ADICIONE ISTO: Ativar o CORS e mapear as Controllers
app.UseCors();
app.UseAuthorization();
app.MapControllers(); 

app.Run();
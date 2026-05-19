namespace PokedexAPI.Models;

public class PokemonDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public List<string> Types { get; set; } = new();
    
    // Novas propriedades para informações básicas
    public int Height { get; set; } // Altura em decímetros
    public int Weight { get; set; } // Peso em hectogramas
    public List<string> Abilities { get; set; } = new(); // Habilidades
}
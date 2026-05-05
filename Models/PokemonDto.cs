namespace PokedexAPI.Models;
public class PokemonDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public List<string> Types { get; set; } = new();
}

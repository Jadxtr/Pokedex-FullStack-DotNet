using PokedexAPI.Models;
using System.Net.Http.Json;

namespace PokedexAPI.Services;

public class PokemonService
{
    private readonly HttpClient _httpClient;

    public PokemonService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PokemonDto>> GetStarterPokemonsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<PokeApiListResponse>("https://pokeapi.co/api/v2/pokemon?limit=20");
        var pokemonList = new List<PokemonDto>();

        if (response != null && response.Results != null)
        {
            foreach (var item in response.Results)
            {
                var details = await _httpClient.GetFromJsonAsync<PokeApiDetailResponse>(item.Url);

                if (details != null)
                {
                    pokemonList.Add(new PokemonDto
                    {
                        Id = details.Id,
                        Name = details.Name,
                        ImageUrl = details.Sprites.Other.OfficialArtwork.FrontDefault,
                        Types = details.Types.Select(t => t.Type.Name).ToList(),
                        Height = details.Height,
                        Weight = details.Weight,
                        Abilities = details.Abilities.Select(a => a.Ability.Name).ToList()
                    });
                }
            }
        }

        return pokemonList;
    }
}

// Classes espelho para desserialização da PokeAPI
public class PokeApiListResponse
{
    public List<PokeApiResult> Results { get; set; } = new();
}

public class PokeApiResult
{
    public string Name { get; set; } = default!;
    public string Url { get; set; } = default!;
}

public class PokeApiDetailResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public PokemonSprites Sprites { get; set; } = default!;
    public List<TypeSlot> Types { get; set; } = new();
    public int Height { get; set; }
    public int Weight { get; set; }
    public List<AbilitySlot> Abilities { get; set; } = new();
}

public class PokemonSprites
{
    public OtherSprites Other { get; set; } = default!;
}

public class OtherSprites
{
    [System.Text.Json.Serialization.JsonPropertyName("official-artwork")]
    public OfficialArtwork OfficialArtwork { get; set; } = default!;
}

public class OfficialArtwork
{
    [System.Text.Json.Serialization.JsonPropertyName("front_default")]
    public string FrontDefault { get; set; } = default!;
}

public class TypeSlot { public TypeInfo Type { get; set; } = default!; }
public class TypeInfo { public string Name { get; set; } = default!; }

public class AbilitySlot { public AbilityInfo Ability { get; set; } = default!; }
public class AbilityInfo { public string Name { get; set; } = default!; }
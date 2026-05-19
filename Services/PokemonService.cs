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
        // 1. Faz a chamada para a lista inicial (limite de 20 para performance)
        var response = await _httpClient.GetFromJsonAsync<PokeApiListResponse>("https://pokeapi.co/api/v2/pokemon?limit=20");
        
        var pokemonList = new List<PokemonDto>();

        if (response != null && response.Results != null)
        {
            foreach (var item in response.Results)
            {
                // 2. Para cada um, busca os detalhes completos
                var detail = await _httpClient.GetFromJsonAsync<PokeApiDetailResponse>(item.Url);

                if (detail != null)
                {
                    pokemonList.Add(new PokemonDto
                    {
                        Id = detail.Id,
                        Name = detail.Name,
                        ImageUrl = detail.Sprites.Other?.OfficialArtwork?.FrontDefault ?? detail.Sprites.Front_default,
                        Types = detail.Types.Select(t => t.Type.Name).ToList(),
                        Height = detail.Height,
                        Weight = detail.Weight,
                        Abilities = detail.Abilities.Select(a => a.Ability.Name).ToList()
                    });
                }
            }
        }

        return pokemonList;
    }
}

// Classes auxiliares para desserializar o JSON da PokeAPI
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
    public Sprites Sprites { get; set; } = default!;
    public List<TypeSlot> Types { get; set; } = default!;
    public int Height { get; set; }
    public int Weight { get; set; }
    public List<AbilitySlot> Abilities { get; set; } = default!;
}

public class Sprites 
{ 
    public string Front_default { get; set; } = default!; 
    public Other Other { get; set; } = default!; 
}

public class Other 
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
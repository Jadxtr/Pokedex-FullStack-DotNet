using PokedexAPI.Models; // Esta linha avisa ao serviço onde procurar o Dto
using System.Net.Http.Json;
using System.Text.Json.Serialization;
namespace PokedexAPI.Services;

public class PokemonService
{
    private readonly HttpClient _httpClient;

    // Construtor: O ASP.NET injeta o HttpClient aqui automaticamente
    public PokemonService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PokemonDto>> GetStarterPokemonsAsync()
    {
        // 1. Faz a chamada para a lista (limite de 20 para ser rápido)
        var response = await _httpClient.GetFromJsonAsync<PokeApiListResponse>("https://pokeapi.co/api/v2/pokemon?limit=20");
        
        var pokemonList = new List<PokemonDto>();

        if (response != null && response.Results != null)
        {
            foreach (var item in response.Results)
            {
                // 2. Para cada um, busca os detalhes (onde está a foto)
                var details = await _httpClient.GetFromJsonAsync<PokeApiDetailResponse>(item.Url);
                
                if (details != null)
                {
                    pokemonList.Add(new PokemonDto
                    {
                        Id = details.Id,
                        Name = details.Name,
                        // URL da imagem oficial (mais bonita que o sprite antigo)
                        ImageUrl = details.Sprites.Other.OfficialArtwork.FrontDefault,
                        Types = details.Types.Select(t => t.Type.Name).ToList()
                    });
                }
            }
        }

        return pokemonList;
    }
}

// Essas classes aqui embaixo servem apenas para o C# entender o JSON da PokeAPI
public class PokeApiListResponse { public List<PokeApiResult> Results { get; set; } }
public class PokeApiResult { public string Name { get; set; } public string Url { get; set; } }
public class PokeApiDetailResponse { 
    public int Id { get; set; } 
    public string Name { get; set; } 
    public Sprites Sprites { get; set; } 
    public List<TypeSlot> Types { get; set; }
}
public class Sprites { public Other Other { get; set; } }
// Certifique-se de que o nome da propriedade na classe Other 
// seja igual ao que você chama no código (OfficialArtwork)
public class Other 
{ 
    [JsonPropertyName("official-artwork")]
    public OfficialArtwork OfficialArtwork { get; set; } = default!;
}

public class OfficialArtwork 
{ 
    [JsonPropertyName("front_default")]
    public string FrontDefault { get; set; } = default!;
}
public class TypeSlot { public TypeInfo Type { get; set; } }
public class TypeInfo { public string Name { get; set; } }
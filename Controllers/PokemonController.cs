using Microsoft.AspNetCore.Mvc;
using PokedexAPI.Models;
using PokedexAPI.Services;

namespace PokedexAPI.Controllers;

[ApiController]
[Route("api/[controller]")] // O endereço será api/pokemon
public class PokemonController : ControllerBase
{
    private readonly PokemonService _pokemonService;

    // O ASP.NET entrega o serviço pronto aqui através do construtor
    public PokemonController(PokemonService pokemonService)
    {
        _pokemonService = pokemonService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PokemonDto>>> Get()
    {
        try
        {
            var pokemons = await _pokemonService.GetStarterPokemonsAsync();
            return Ok(pokemons); // Retorna a lista com status 200 (Sucesso)
        }
        catch (Exception ex)
        {
            // Caso algo dê errado (ex: sem internet), retorna o erro
            return StatusCode(500, $"Erro ao buscar dados: {ex.Message}");
        }
    }
}
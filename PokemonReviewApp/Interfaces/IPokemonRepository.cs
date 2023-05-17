using Pokemon_Review_System.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
         ICollection<Pokemon> GetAllPokemon();
         Pokemon GetPokemonById(int pokemonId);
         Pokemon GetPokemonByName(string pokemonName);
         decimal GetPokemonRating (int pokemonId);
         bool IsPokemonExist(int pokemonId);
    }
}

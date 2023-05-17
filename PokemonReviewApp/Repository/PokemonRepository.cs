using Pokemon_Review_System.Data;
using Pokemon_Review_System.Models;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly PokemonContext _context;
        public PokemonRepository(PokemonContext context) 
        {
         _context = context;
        }

        public ICollection<Pokemon> GetAllPokemon()
        {
            return _context.Pokemon.OrderBy(p=>p.Id).ToList();
        }

        Pokemon IPokemonRepository.GetPokemonById(int pokemonId)
        {
           return _context.Pokemon.Where(p => p.Id == pokemonId).FirstOrDefault();
        }

        Pokemon IPokemonRepository.GetPokemonByName(string pokemonName)
        {
           return _context.Pokemon.Where(p => p.Name == pokemonName).FirstOrDefault();
        }

        decimal IPokemonRepository.GetPokemonRating(int pokemonId)
        {
            var reviews = _context.Reviews.Where(r => r.Pokemon.Id == pokemonId);
            if(reviews.Count() <= 0)
                return 0;
            //return the average of all the ratings
            return ((decimal)reviews.Sum(r => r.Rating) / reviews.Count());
              
        }

        bool IPokemonRepository.IsPokemonExist(int pokemonId)
        {
            return _context.Pokemon.Any(p => p.Id == pokemonId);
        }
    }
}
